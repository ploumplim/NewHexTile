// Made with Amplify Shader Editor v1.9.3.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "VFXWATER"
{
	Properties
	{
		_TextureSample2("Texture Sample 0", 2D) = "white" {}
		_alphaaqua("alpha aqua", Float) = 0.54
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 50
		_Float1("Float 1", Float) = 1.596457
		_Float2("Float 2", Float) = 0.26
		_Float3("Float 3", Float) = 0.3
		_lerpCrayon("lerpCrayon", Float) = 0
		_TextureSample1("Texture Sample 1", 2D) = "black" {}
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Float7("Float 7", Float) = 1.35
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "AlphaTest+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "Tessellation.cginc"
		#pragma target 4.6
		#pragma surface surf Standard keepalpha noshadow vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldNormal;
			INTERNAL_DATA
			float4 screenPos;
		};

		uniform sampler2D _TextureSample0;
		uniform sampler2D _TextureSample2;
		uniform float _Float3;
		uniform float _Float2;
		uniform float _lerpCrayon;
		uniform float _alphaaqua;
		uniform sampler2D _TextureSample1;
		uniform float _Float1;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _Float7;
		uniform float _EdgeLength;


		struct Gradient
		{
			int type;
			int colorsLength;
			int alphasLength;
			float4 colors[8];
			float2 alphas[8];
		};


		Gradient NewGradient(int type, int colorsLength, int alphasLength, 
		float4 colors0, float4 colors1, float4 colors2, float4 colors3, float4 colors4, float4 colors5, float4 colors6, float4 colors7,
		float2 alphas0, float2 alphas1, float2 alphas2, float2 alphas3, float2 alphas4, float2 alphas5, float2 alphas6, float2 alphas7)
		{
			Gradient g;
			g.type = type;
			g.colorsLength = colorsLength;
			g.alphasLength = alphasLength;
			g.colors[ 0 ] = colors0;
			g.colors[ 1 ] = colors1;
			g.colors[ 2 ] = colors2;
			g.colors[ 3 ] = colors3;
			g.colors[ 4 ] = colors4;
			g.colors[ 5 ] = colors5;
			g.colors[ 6 ] = colors6;
			g.colors[ 7 ] = colors7;
			g.alphas[ 0 ] = alphas0;
			g.alphas[ 1 ] = alphas1;
			g.alphas[ 2 ] = alphas2;
			g.alphas[ 3 ] = alphas3;
			g.alphas[ 4 ] = alphas4;
			g.alphas[ 5 ] = alphas5;
			g.alphas[ 6 ] = alphas6;
			g.alphas[ 7 ] = alphas7;
			return g;
		}


		float4 SampleGradient( Gradient gradient, float time )
		{
			float3 color = gradient.colors[0].rgb;
			UNITY_UNROLL
			for (int c = 1; c < 8; c++)
			{
			float colorPos = saturate((time - gradient.colors[c-1].w) / ( 0.00001 + (gradient.colors[c].w - gradient.colors[c-1].w)) * step(c, (float)gradient.colorsLength-1));
			color = lerp(color, gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), gradient.type));
			}
			#ifndef UNITY_COLORSPACE_GAMMA
			color = half3(GammaToLinearSpaceExact(color.r), GammaToLinearSpaceExact(color.g), GammaToLinearSpaceExact(color.b));
			#endif
			float alpha = gradient.alphas[0].x;
			UNITY_UNROLL
			for (int a = 1; a < 8; a++)
			{
			float alphaPos = saturate((time - gradient.alphas[a-1].y) / ( 0.00001 + (gradient.alphas[a].y - gradient.alphas[a-1].y)) * step(a, (float)gradient.alphasLength-1));
			alpha = lerp(alpha, gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), gradient.type));
			}
			return float4(color, alpha);
		}


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		void vertexDataFunc( inout appdata_full v )
		{
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 panner153 = ( _Time.y * float2( 0,0 ) + i.uv_texcoord);
			o.Normal = tex2D( _TextureSample0, panner153 ).rgb;
			float mulTime80 = _Time.y * 0.13;
			float cos166 = cos( mulTime80 );
			float sin166 = sin( mulTime80 );
			float2 rotator166 = mul( i.uv_texcoord - float2( 0.5,0.5 ) , float2x2( cos166 , -sin166 , sin166 , cos166 )) + float2( 0.5,0.5 );
			float4 tex2DNode107 = tex2D( _TextureSample2, rotator166 );
			Gradient gradient100 = NewGradient( 2, 2, 2, float4( 1, 1, 1, 0 ), float4( 0, 0, 0, 1 ), 0, 0, 0, 0, 0, 0, float2( 1, 0 ), float2( 1, 1 ), 0, 0, 0, 0, 0, 0 );
			Gradient gradient95 = NewGradient( 0, 2, 2, float4( 1, 1, 1, 0 ), float4( 0, 0, 0, 1 ), 0, 0, 0, 0, 0, 0, float2( 1, 0 ), float2( 1, 1 ), 0, 0, 0, 0, 0, 0 );
			float cos168 = cos( _Time.y );
			float sin168 = sin( _Time.y );
			float2 rotator168 = mul( i.uv_texcoord - float2( 0.5,0.5 ) , float2x2( cos168 , -sin168 , sin168 , cos168 )) + float2( 0.5,0.5 );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_normWorldNormal = normalize( ase_worldNormal );
			float4 lerpResult101 = lerp( SampleGradient( gradient95, ( float3( rotator168 ,  0.0 ) * (ase_normWorldNormal*5.11 + float3( rotator168 ,  0.0 )) ).x ) , float4( 0,0,0,0 ) , _Float3);
			float div104=256.0/float(8);
			float4 posterize104 = ( floor( ( SampleGradient( gradient95, ( float3( rotator168 ,  0.0 ) * (ase_normWorldNormal*5.11 + float3( rotator168 ,  0.0 )) ).x ) * _Float2 ) * div104 ) / div104 );
			float4 lerpResult110 = lerp( SampleGradient( gradient100, lerpResult101.r ) , posterize104 , _lerpCrayon);
			float4 temp_output_115_0 = ( lerpResult110 * tex2DNode107 * 0.37 );
			float4 lerpResult113 = lerp( SampleGradient( gradient100, lerpResult101.r ) , posterize104 , _alphaaqua);
			float2 panner79 = ( mulTime80 * float2( 0.95,0 ) + i.uv_texcoord);
			float div149=256.0/float(76);
			float4 posterize149 = ( floor( tex2D( _TextureSample1, panner79 ) * div149 ) / div149 );
			float2 panner148 = ( _Time.y * float2( 0.1,0.12 ) + i.uv_texcoord);
			float simplePerlin2D139 = snoise( panner148*1.39 );
			simplePerlin2D139 = simplePerlin2D139*0.5 + 0.5;
			o.Albedo = ( tex2DNode107 * max( temp_output_115_0 , ( lerpResult113 + ( ( 1.0 - posterize149 ) * _Float1 ) + simplePerlin2D139 ) ) ).rgb;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth157 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth157 = abs( ( screenDepth157 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( 0.43 ) );
			float clampResult162 = clamp( ( ( 1.0 - distanceDepth157 ) * _Float7 ) , 0.0 , 1.0 );
			float3 temp_cast_9 = (clampResult162).xxx;
			o.Emission = temp_cast_9;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19302
Node;AmplifyShaderEditor.Vector2Node;169;-2605.963,-899.8777;Inherit;False;Constant;_Vector5;Vector 4;12;0;Create;True;0;0;0;False;0;False;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;170;-2624.538,-726.3994;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;171;-2759.525,-706.9388;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;89;-3821.407,-534.9155;Inherit;True;True;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;91;-3348.212,-166.3553;Inherit;False;Constant;_Float4;Float 4;12;0;Create;True;0;0;0;False;0;False;5.11;-1.16;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;168;-3185.964,-795.8778;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;92;-3005.485,-638.7855;Inherit;True;3;0;FLOAT3;0,0,0;False;1;FLOAT;-1;False;2;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;77;-1215.231,-21.81442;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;80;-1251.218,285.6461;Inherit;False;1;0;FLOAT;0.13;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;81;-1180.103,138.0872;Inherit;False;Constant;_Vector0;Vector 0;2;0;Create;True;0;0;0;False;0;False;0.95,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;172;-2077.633,-713.822;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GradientNode;95;-2021.868,-967.066;Inherit;False;0;2;2;1,1,1,0;0,0,0,1;1,0;1,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.PannerNode;79;-645.9661,-14.47051;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;97;-1580.027,-453.1553;Inherit;False;Property;_Float2;Float 2;10;0;Create;True;0;0;0;False;0;False;0.26;0.56;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GradientSampleNode;98;-1828.471,-819.0471;Inherit;True;2;0;OBJECT;;False;1;FLOAT;3.71;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;96;-1370.544,-710.0247;Inherit;False;Property;_Float3;Float 3;11;0;Create;True;0;0;0;False;0;False;0.3;0.83;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;102;-423.1944,-58.80368;Inherit;True;Property;_TextureSample1;Texture Sample 1;13;0;Create;True;0;0;0;False;0;False;-1;None;b13ee954d39510046ab9ecc014856d8d;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;167;-1196.656,-195.2929;Inherit;False;Constant;_Vector4;Vector 4;12;0;Create;True;0;0;0;False;0;False;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;99;-1289.387,-648.2286;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;101;-1096.763,-758.6572;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GradientNode;100;-1321.691,-900.3151;Inherit;False;2;2;2;1,1,1,0;0,0,0,1;1,0;1,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.SimpleTimeNode;145;-402.6295,597.3081;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;146;-440.9144,383.3492;Inherit;False;Constant;_Vector1;Vector 0;2;0;Create;True;0;0;0;False;0;False;0.1,0.12;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;147;-545.2419,146.1476;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosterizeNode;149;-109.6392,-101.1005;Inherit;True;76;2;1;COLOR;0,0,0,0;False;0;INT;76;False;1;COLOR;0
Node;AmplifyShaderEditor.RotatorNode;166;-966.656,-149.2929;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;158;1072.598,578.8124;Inherit;False;Constant;_Float6;Float 6;10;0;Create;True;0;0;0;False;0;False;0.43;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GradientSampleNode;103;-886.7057,-901.644;Inherit;True;2;0;OBJECT;;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosterizeNode;104;-927.2849,-367.5977;Inherit;True;8;2;1;COLOR;0,0,0,0;False;0;INT;8;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;105;-770.4816,-588.1681;Inherit;False;Property;_lerpCrayon;lerpCrayon;12;0;Create;True;0;0;0;False;0;False;0;0.91;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;106;-683.2566,-177.6567;Inherit;False;Property;_alphaaqua;alpha aqua;2;0;Create;True;0;0;0;False;0;False;0.54;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;108;143.8126,-141.1442;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PannerNode;148;-199.5774,282.3915;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;109;200.786,6.61631;Inherit;False;Property;_Float1;Float 1;9;0;Create;True;0;0;0;False;0;False;1.596457;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;157;1261.982,472.4023;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;110;-428.1299,-733.6448;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;112;194.1854,-396.5072;Inherit;False;Constant;_Float0;Float 0;11;0;Create;True;0;0;0;False;0;False;0.37;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;107;-128.1845,-557.1369;Inherit;True;Property;_TextureSample2;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;2916e2e52d9b7bb4bb88871adfe1a224;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;0.42;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NoiseGeneratorNode;139;30.7995,115.1652;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1.39;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;113;-152.9923,-335.3119;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;114;413.1053,-183.0869;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;159;1533.2,447.1624;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;161;1700.959,581.599;Inherit;False;Property;_Float7;Float 7;15;0;Create;True;0;0;0;False;0;False;1.35;0.09;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;152;1034.934,-107.2688;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;154;1220.946,285.1918;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;155;1048.061,53.63282;Inherit;False;Constant;_Vector3;Vector 0;2;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;115;453.4459,-582.406;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;116;728.7104,-291.0341;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;160;1745.373,275.4146;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;153;1325.198,-86.92485;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;118;1401.869,-432.4003;Inherit;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;176;1604.942,-612.7709;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldSpaceLightPos;93;-2512.478,-367.6864;Inherit;False;0;3;FLOAT4;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleAddOpNode;94;-2401.673,-665.4854;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldSpaceLightPos;165;-3868.483,-135.8376;Inherit;True;0;3;FLOAT4;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.DotProductOpNode;90;-3346.477,-434.0079;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;117;-96.61436,-739.2283;Inherit;False;Property;_Color0;Color 0;3;0;Create;True;0;0;0;False;0;False;0.8584906,0.7484342,0.5871751,0;0.145098,0.5843138,0.5168866,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;130;767.039,273.8624;Inherit;False;Constant;_Vector2;Vector 2;11;0;Create;True;0;0;0;False;0;False;0.5,0.5,0.5;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ClampOpNode;162;1924.004,-23.33715;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosterizeNode;144;693.3884,45.16019;Inherit;False;91;2;1;COLOR;0,0,0,0;False;0;INT;91;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;120;1198.435,-233.9758;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;131;1587.994,82.02613;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;151;1537.137,-104.0889;Inherit;True;Property;_TextureSample0;Texture Sample 0;14;0;Create;True;0;0;0;False;0;False;-1;None;b13ee954d39510046ab9ecc014856d8d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;111;124.3486,-693.4424;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;119;1821.43,-425.1753;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2142.783,-129.9716;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;VFXWATER;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Custom;0.15;True;False;0;True;Opaque;;AlphaTest;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;True;2;50;10;25;False;0.5;False;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;1;-1;-1;4;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;17;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;16;FLOAT4;0,0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;168;0;170;0
WireConnection;168;1;169;0
WireConnection;168;2;171;0
WireConnection;92;0;89;0
WireConnection;92;1;91;0
WireConnection;92;2;168;0
WireConnection;172;0;168;0
WireConnection;172;1;92;0
WireConnection;79;0;77;0
WireConnection;79;2;81;0
WireConnection;79;1;80;0
WireConnection;98;0;95;0
WireConnection;98;1;172;0
WireConnection;102;1;79;0
WireConnection;99;0;98;0
WireConnection;99;1;97;0
WireConnection;101;0;98;0
WireConnection;101;2;96;0
WireConnection;149;1;102;0
WireConnection;166;0;77;0
WireConnection;166;1;167;0
WireConnection;166;2;80;0
WireConnection;103;0;100;0
WireConnection;103;1;101;0
WireConnection;104;1;99;0
WireConnection;108;0;149;0
WireConnection;148;0;147;0
WireConnection;148;2;146;0
WireConnection;148;1;145;0
WireConnection;157;0;158;0
WireConnection;110;0;103;0
WireConnection;110;1;104;0
WireConnection;110;2;105;0
WireConnection;107;1;166;0
WireConnection;139;0;148;0
WireConnection;113;0;103;0
WireConnection;113;1;104;0
WireConnection;113;2;106;0
WireConnection;114;0;108;0
WireConnection;114;1;109;0
WireConnection;159;0;157;0
WireConnection;115;0;110;0
WireConnection;115;1;107;0
WireConnection;115;2;112;0
WireConnection;116;0;113;0
WireConnection;116;1;114;0
WireConnection;116;2;139;0
WireConnection;160;0;159;0
WireConnection;160;1;161;0
WireConnection;153;0;152;0
WireConnection;153;2;155;0
WireConnection;153;1;154;0
WireConnection;118;0;115;0
WireConnection;118;1;116;0
WireConnection;176;0;115;0
WireConnection;176;1;107;0
WireConnection;94;1;93;1
WireConnection;90;0;89;0
WireConnection;90;1;165;1
WireConnection;162;0;160;0
WireConnection;144;1;139;0
WireConnection;131;0;139;0
WireConnection;131;1;130;0
WireConnection;151;1;153;0
WireConnection;119;0;107;0
WireConnection;119;1;118;0
WireConnection;0;0;119;0
WireConnection;0;1;151;0
WireConnection;0;2;162;0
ASEEND*/
//CHKSM=48FD6A4359AF5ED29707B4BF9A4F2E97AF1624DC