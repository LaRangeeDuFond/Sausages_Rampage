Shader "GLSL anisotropic per-pixel lighting" {
   Properties {
      _Color ("Diffuse Material Color", Color) = (1,1,1,1) 
      _SpecColor ("Specular Material Color", Color) = (1,1,1,1) 
      _AlphaX ("Roughness in Brush Direction", Float) = 1.0
      _AlphaY ("Roughness orthogonal to Brush Direction", Float) = 1.0
   }
   SubShader {
      Pass {	
         Tags { "LightMode" = "ForwardBase" } 
            // pass for ambient light and first light source
 
         GLSLPROGRAM
 
         // User-specified properties
         uniform vec4 _Color; 
         uniform vec4 _SpecColor; 
         uniform float _AlphaX;
         uniform float _AlphaY;
 
         // The following built-in uniforms (except _LightColor0) 
         // are also defined in "UnityCG.glslinc", 
         // i.e. one could #include "UnityCG.glslinc" 
         uniform vec3 _WorldSpaceCameraPos; 
            // camera position in world space
         uniform mat4 _Object2World; // model matrix
         uniform mat4 _World2Object; // inverse model matrix
         uniform vec4 _WorldSpaceLightPos0; 
            // direction to or position of light source
         uniform vec4 _LightColor0; 
            // color of light source (from "Lighting.cginc")
 
         varying vec4 position; 
            // position of the vertex (and fragment) in world space 
         varying vec3 varyingNormalDirection; 
            // surface normal vector in world space
         varying vec3 varyingTangentDirection; 
            // brush direction in world space
 
         #ifdef VERTEX
 
         attribute vec4 Tangent; // tangent vector provided 
            // by Unity (used as brush direction)
 
         void main()
         {				
            mat4 modelMatrix = _Object2World;
            mat4 modelMatrixInverse = _World2Object; // unity_Scale.w 
               // is unnecessary because we normalize vectors
 
            position = modelMatrix * gl_Vertex;
            varyingNormalDirection = normalize(vec3(
               vec4(gl_Normal, 0.0) * modelMatrixInverse));
            varyingTangentDirection = normalize(vec3(
               modelMatrix * vec4(vec3(Tangent), 0.0)));
 
            gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
         }
 
         #endif
 
         #ifdef FRAGMENT
 
         void main()
         {
            vec3 normalDirection = normalize(varyingNormalDirection);
            vec3 tangentDirection = normalize(varyingTangentDirection);
 
            vec3 viewDirection = 
               normalize(_WorldSpaceCameraPos - vec3(position));
            vec3 lightDirection;
            float attenuation;
 
            if (0.0 == _WorldSpaceLightPos0.w) // directional light?
            {
               attenuation = 1.0; // no attenuation
               lightDirection = normalize(vec3(_WorldSpaceLightPos0));
            } 
            else // point or spot light
            {
               vec3 vertexToLightSource = 
                  vec3(_WorldSpaceLightPos0 - position);
               float distance = length(vertexToLightSource);
               attenuation = 1.0 / distance; // linear attenuation 
               lightDirection = normalize(vertexToLightSource);
            }
 
            vec3 halfwayVector = 
               normalize(lightDirection + viewDirection);
	    vec3 binormalDirection = 
               cross(normalDirection, tangentDirection);
            float dotLN = dot(lightDirection, normalDirection); 
               // compute this dot product only once
 
            vec3 ambientLighting = 
               vec3(gl_LightModel.ambient) * vec3(_Color);
 
            vec3 diffuseReflection = attenuation * vec3(_LightColor0) 
               * vec3(_Color) * max(0.0, dotLN);
 
            vec3 specularReflection;
            if (dotLN < 0.0) // light source on the wrong side?
            {
               specularReflection = vec3(0.0, 0.0, 0.0); 
                  // no specular reflection
            }
            else // light source on the right side
            {
               float dotHN = dot(halfwayVector, normalDirection);
               float dotVN = dot(viewDirection, normalDirection);
               float dotHTAlphaX = 
                  dot(halfwayVector, tangentDirection) / _AlphaX;
               float dotHBAlphaY = 
                  dot(halfwayVector, binormalDirection) / _AlphaY;
 
               specularReflection = attenuation * vec3(_SpecColor) 
                  * sqrt(max(0.0, dotLN / dotVN)) 
                  * exp(-2.0 * (dotHTAlphaX * dotHTAlphaX 
                  + dotHBAlphaY * dotHBAlphaY) / (1.0 + dotHN));
            }
 
            gl_FragColor = vec4(ambientLighting 
               + diffuseReflection + specularReflection, 1.0);
         }
 
         #endif
 
         ENDGLSL
      }
 
      Pass {	
         Tags { "LightMode" = "ForwardAdd" } 
            // pass for additional light sources
         Blend One One // additive blending 
 
 
         GLSLPROGRAM
 
         // User-specified properties
         uniform vec4 _Color; 
         uniform vec4 _SpecColor; 
         uniform float _AlphaX;
         uniform float _AlphaY;
 
         // The following built-in uniforms (except _LightColor0) 
         // are also defined in "UnityCG.glslinc", 
         // i.e. one could #include "UnityCG.glslinc" 
         uniform vec3 _WorldSpaceCameraPos; 
            // camera position in world space
         uniform mat4 _Object2World; // model matrix
         uniform mat4 _World2Object; // inverse model matrix
         uniform vec4 _WorldSpaceLightPos0; 
            // direction to or position of light source
         uniform vec4 _LightColor0; 
            // color of light source (from "Lighting.cginc")
 
         varying vec4 position; 
            // position of the vertex (and fragment) in world space 
         varying vec3 varyingNormalDirection; 
            // surface normal vector in world space
         varying vec3 varyingTangentDirection; 
            // brush direction in world space
 
         #ifdef VERTEX
 
         attribute vec4 Tangent; // tangent vector provided 
            // by Unity (used as brush direction)
 
         void main()
         {				
            mat4 modelMatrix = _Object2World;
            mat4 modelMatrixInverse = _World2Object; // unity_Scale.w 
               // is unnecessary because we normalize vectors
 
            position = modelMatrix * gl_Vertex;
            varyingNormalDirection = normalize(vec3(
               vec4(gl_Normal, 0.0) * modelMatrixInverse));
            varyingTangentDirection = normalize(vec3(
               modelMatrix * vec4(vec3(Tangent), 0.0)));
 
            gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
         }
 
         #endif
 
         #ifdef FRAGMENT
 
         void main()
         {
            vec3 normalDirection = normalize(varyingNormalDirection);
            vec3 tangentDirection = normalize(varyingTangentDirection);
 
            vec3 viewDirection = 
               normalize(_WorldSpaceCameraPos - vec3(position));
            vec3 lightDirection;
            float attenuation;
 
            if (0.0 == _WorldSpaceLightPos0.w) // directional light?
            {
               attenuation = 1.0; // no attenuation
               lightDirection = normalize(vec3(_WorldSpaceLightPos0));
            } 
            else // point or spot light
            {
               vec3 vertexToLightSource = 
                  vec3(_WorldSpaceLightPos0 - position);
               float distance = length(vertexToLightSource);
               attenuation = 1.0 / distance; // linear attenuation 
               lightDirection = normalize(vertexToLightSource);
            }
 
            vec3 halfwayVector = 
               normalize(lightDirection + viewDirection);
	    vec3 binormalDirection = 
               cross(normalDirection, tangentDirection);
            float dotLN = dot(lightDirection, normalDirection); 
               // compute this dot product only once
 
            vec3 diffuseReflection = attenuation * vec3(_LightColor0) 
               * vec3(_Color) * max(0.0, dotLN);
 
            vec3 specularReflection;
            if (dotLN < 0.0) // light source on the wrong side?
            {
               specularReflection = vec3(0.0, 0.0, 0.0); 
                  // no specular reflection
            }
            else // light source on the right side
            {
               float dotHN = dot(halfwayVector, normalDirection);
               float dotVN = dot(viewDirection, normalDirection);
               float dotHTAlphaX = 
                  dot(halfwayVector, tangentDirection) / _AlphaX;
               float dotHBAlphaY = 
                  dot(halfwayVector, binormalDirection) / _AlphaY;
 
               specularReflection = attenuation * vec3(_SpecColor) 
                  * sqrt(max(0.0, dotLN / dotVN)) 
                  * exp(-2.0 * (dotHTAlphaX * dotHTAlphaX 
                  + dotHBAlphaY * dotHBAlphaY) / (1.0 + dotHN));
            }
 
            gl_FragColor = 
               vec4(diffuseReflection + specularReflection, 1.0);
         }
 
         #endif
 
         ENDGLSL
      }
   } 
   // The definition of a fallback shader should be commented out 
   // during development:
   // Fallback "Specular"
}