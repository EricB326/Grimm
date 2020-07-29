float _Frequency;
float _Amplitude;
float _Speed;

// Attributes struct has to be redefined to change the type of positionOS to
// float4; It's originally defined as float3. Also previousPositionOS is added 
// to support the motion vectors pass.
struct Attributes
{
    float4 positionOS   : POSITION;
#ifdef ATTRIBUTES_NEED_NORMAL
    float3 normalOS     : NORMAL;
#endif
#ifdef ATTRIBUTES_NEED_TANGENT
    float4 tangentOS    : TANGENT; // Sign is stored with the W.
#endif
#ifdef ATTRIBUTES_NEED_TEXCOORD0
    float2 uv0          : TEXCOORD0;
#endif
#ifdef ATTRIBUTES_NEED_TEXCOORD1
    float2 uv1          : TEXCOORD1;
#endif
#ifdef ATTRIBUTES_NEED_TEXCOORD2
    float2 uv2          : TEXCOORD2;
#endif
#ifdef ATTRIBUTES_NEED_TEXCOORD3
    float2 uv3          : TEXCOORD3;
#endif
#if SHADERPASS == SHADERPASS_MOTION_VECTORS
    float3 previousPositionOS : TEXCOORD4; // Contain previous transform position (in case of skinning for example)
#endif
#ifdef ATTRIBUTES_NEED_COLOR
    float4 color        : COLOR;
#endif

    UNITY_VERTEX_INPUT_INSTANCE_ID
};

AttributesMesh ConvertToAttributesMesh(Attributes input)
{
    AttributesMesh am;
    am.positionOS = input.positionOS.xyz;
#ifdef ATTRIBUTES_NEED_NORMAL
    am.normalOS = input.normalOS;
#endif
#ifdef ATTRIBUTES_NEED_TANGENT
    am.tangentOS = input.tangentOS;
#endif
#ifdef ATTRIBUTES_NEED_TEXCOORD0
    am.uv0 = input.uv0;
#endif
#ifdef ATTRIBUTES_NEED_TEXCOORD1
    am.uv1 = input.uv1;
#endif
#ifdef ATTRIBUTES_NEED_TEXCOORD2
    am.uv2 = input.uv2;
#endif
#ifdef ATTRIBUTES_NEED_TEXCOORD3
    am.uv3 = input.uv3;
#endif
#ifdef ATTRIBUTES_NEED_COLOR
    am.color = input.color;
#endif
    UNITY_TRANSFER_INSTANCE_ID(input, am);
    return am;
}

// TODO: Here we will also have all the vertex deformation (GPU skinning, vertex animation, morph target...) or we will need to generate a compute shaders instead (better! but require work to deal with unpacking like fp16)
// Make it inout so that MotionVectorPass can get the modified input values later.

VaryingsMeshType VertAnimMesh(AttributesMesh input)
{
    VaryingsMeshType output;

    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_TRANSFER_INSTANCE_ID(input, output);

#if defined(HAVE_MESH_MODIFICATION)
    input = ApplyMeshModification(input, _TimeParameters.xyz);
#endif

    // This return the camera relative position (if enable)
    float3 positionRWS = TransformObjectToWorld(input.positionOS);
#ifdef ATTRIBUTES_NEED_NORMAL
    float3 normalWS = TransformObjectToWorldNormal(input.normalOS);
#else
    float3 normalWS = float3(0.0, 0.0, 0.0); // We need this case to be able to compile ApplyVertexModification that doesn't use normal.
#endif

#ifdef ATTRIBUTES_NEED_TANGENT
    float4 tangentWS = float4(TransformObjectToWorldDir(input.tangentOS.xyz), input.tangentOS.w);
#endif

    // Do vertex modification in camera relative space (if enable)
#if defined(HAVE_VERTEX_MODIFICATION)
    ApplyVertexModification(input, normalWS, positionRWS, _TimeParameters.xyz);
#endif

#ifdef TESSELLATION_ON
    output.positionRWS = positionRWS;
    output.normalWS = normalWS;
#if defined(VARYINGS_NEED_TANGENT_TO_WORLD) || defined(VARYINGS_DS_NEED_TANGENT)
    output.tangentWS = tangentWS;
#endif
#else
#ifdef VARYINGS_NEED_POSITION_WS
    output.positionRWS = positionRWS;
#endif
    output.positionCS = TransformWorldToHClip(positionRWS);
#ifdef VARYINGS_NEED_TANGENT_TO_WORLD
    output.normalWS = normalWS;
    output.tangentWS = tangentWS;
#endif
#endif

#if defined(VARYINGS_NEED_TEXCOORD0) || defined(VARYINGS_DS_NEED_TEXCOORD0)
    output.texCoord0 = input.uv0;
#endif
#if defined(VARYINGS_NEED_TEXCOORD1) || defined(VARYINGS_DS_NEED_TEXCOORD1)
    output.texCoord1 = input.uv1;
#endif
#if defined(VARYINGS_NEED_TEXCOORD2) || defined(VARYINGS_DS_NEED_TEXCOORD2)
    output.texCoord2 = input.uv2;
#endif
#if defined(VARYINGS_NEED_TEXCOORD3) || defined(VARYINGS_DS_NEED_TEXCOORD3)
    output.texCoord3 = input.uv3;
#endif
#if defined(VARYINGS_NEED_COLOR) || defined(VARYINGS_DS_NEED_COLOR)
    output.color = input.color;
#endif

    return output;
}

//PackedVaryingsType VertAnimVertex(AttributesMesh inputMesh)
//{
//    inputMesh.positionOS.z += sin((inputMesh.uv0.x - (_Time.y * 1)) * 1) * (inputMesh.uv0.x * 1);
//    VaryingsType varyingsType;
//    varyingsType.vmesh = VertAnimMesh(inputMesh);
//    return PackVaryingsType(varyingsType);
//}

// Final vertex data packing before being sent off to the base Vert() function
//PackedVaryingsType PackVertexData(AttributesMesh source, float3 position, float3 position_prev, float3 normal, float4 color)
//{
//    source.positionOS = position;
//
//    #if defined(VARYINGS_NEED_TEXCOORD1) || defined(VARYINGS_DS_NEED_TEXCOORD1)
//        // FIXME: I'm not sure why but the shader compiler emits an "unexpected
//        // LEFT_BRACKET" error on Vulkan. Strangely, it disappears by touching UV1
//        // before calling VertMesh.
//        source.uv1 = source.uv1 + 1e-12;
//    #endif
//    #ifdef ATTRIBUTES_NEED_NORMAL
//        source.normalOS = normal;
//    #endif
//    #ifdef ATTRIBUTES_NEED_COLOR
//        source.color = color;
//    #endif
//    #if SHADERPASS == SHADERPASS_MOTION_VECTORS
//        AttributesPass attrib;
//        attrib.previousPositionOS = position_prev;
//        return Vert(source, attrib);
//    #else
//        return Vert(source);
//    #endif
//}

// Output the vertex
//PackedVaryingsType VertexOutput(
//    AttributesMesh source,
//    float3 position, float3 position_prev, half3 normal,
//    half emission = 0, half random = 0, half2 qcoord = -1
//)
//{
//    half4 color = half4(qcoord, emission, random);
//    return PackVertexData(source, position, position_prev, normal, color);
//}

PackedVaryingsType VertAnimVertex(AttributesMesh input)
{
//    //float3 p0 = input.positionOS;
//
//#if SHADERPASS == SHADERPASS_MOTION_VECTORS
//    bool hasDeformation = unity_MotionVectorsParams.x > 0.0;
//    float3 p0_prev = hasDeformation ? input.positionOS : p0;
//#else
//    float3 p0_prev = p0;
//#endif
//
//#ifdef ATTRIBUTES_NEED_NORMAL
//    float3 n0 = input.normalOS;
//#else
//    float3 n0 = 0;
//#endif
    
    // To be replaced with: sin((input.uv0.x - (_Time.y * 1)) * 1) * (input.uv0.x * 1);
    //input.positionOS.z += sin((input.uv0.x - (_Time.y * 1)) * 1) * (input.uv0.x * 1);
    VaryingsType varyingsType;
    varyingsType.vmesh = VertAnimMesh(input);
    return PackVaryingsType(varyingsType);
}