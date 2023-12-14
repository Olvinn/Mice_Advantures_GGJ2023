#ifndef MYHLSLINCLUDE_INCLUDED
#define MYHLSLINCLUDE_INCLUDED

#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN

void ShadowAttenuation_float(float3 pos, out float Out)
{
    #if SHADERGRAPH_PREVIEW
    pos = float3(0,0,0);
    Out = 1;
    #else
    #if defined(UNIVERSAL_LIGHTING_INCLUDED)
    float4 shadowCoord = TransformWorldToShadowCoord(pos);
    Light mainLight = GetMainLight(shadowCoord);
    Out = step(.1,mainLight.shadowAttenuation);
    #endif
    #endif
}

#endif