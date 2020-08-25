The HDRP Demo requires: High Definition Render Pipeline (5.16 +) and Postprocessing (2.17 +)

If the demo scene isn’t loading properly, you may need to do one or all of the following:

• “HDRenderPipelineAsset”, found in your project view, may need to be connected to the “Scriptable Render Pipeline Settings” field in the Graphics tab, under Project Settings.

• “Color Space” should be set to Linear (not Gamma) in the Player tab, under Project Settings. 

HDRP settings You will find in: Edit -> Project Settings -> Graphics

Sometimes HDRP doesn't work properly, to fix it go to: Window -> Analysis -> Render Pipeline Wizard and click "Fix"