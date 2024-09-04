#define NUM_TEX_COORD_INTERPOLATORS 1
#define NUM_MATERIAL_TEXCOORDS_VERTEX 1
#define NUM_CUSTOM_VERTEX_INTERPOLATORS 0

struct Input
{
	//float3 Normal;
	float2 uv_MainTex : TEXCOORD0;
	float2 uv2_Material_Texture2D_0 : TEXCOORD1;
	float4 color : COLOR;
	float4 tangent;
	//float4 normal;
	float3 viewDir;
	float4 screenPos;
	float3 worldPos;
	//float3 worldNormal;
	float3 normal2;
};
struct SurfaceOutputStandard
{
	float3 Albedo;		// base (diffuse or specular) color
	float3 Normal;		// tangent space normal, if written
	half3 Emission;
	half Metallic;		// 0=non-metal, 1=metal
	// Smoothness is the user facing name, it should be perceptual smoothness but user should not have to deal with it.
	// Everywhere in the code you meet smoothness it is perceptual smoothness
	half Smoothness;	// 0=rough, 1=smooth
	half Occlusion;		// occlusion (default 1)
	float Alpha;		// alpha for transparencies
};

//#define HDRP 1
#define URP 1
#define UE5
//#define HAS_CUSTOMIZED_UVS 1
#define MATERIAL_TANGENTSPACENORMAL 1
//struct Material
//{
	//samplers start
SAMPLER( SamplerState_Linear_Repeat );
SAMPLER( SamplerState_Linear_Clamp );
TEXTURE2D(       Material_Texture2D_0 );
SAMPLER(  samplerMaterial_Texture2D_0 );
float4 Material_Texture2D_0_TexelSize;
float4 Material_Texture2D_0_ST;
TEXTURE2D(       Material_Texture2D_1 );
SAMPLER(  samplerMaterial_Texture2D_1 );
float4 Material_Texture2D_1_TexelSize;
float4 Material_Texture2D_1_ST;
TEXTURE2D(       Material_Texture2D_2 );
SAMPLER(  samplerMaterial_Texture2D_2 );
float4 Material_Texture2D_2_TexelSize;
float4 Material_Texture2D_2_ST;
TEXTURE2D(       Material_Texture2D_3 );
SAMPLER(  samplerMaterial_Texture2D_3 );
float4 Material_Texture2D_3_TexelSize;
float4 Material_Texture2D_3_ST;
TEXTURE2D(       Material_Texture2D_4 );
SAMPLER(  samplerMaterial_Texture2D_4 );
float4 Material_Texture2D_4_TexelSize;
float4 Material_Texture2D_4_ST;
TEXTURE2D(       Material_Texture2D_5 );
SAMPLER(  samplerMaterial_Texture2D_5 );
float4 Material_Texture2D_5_TexelSize;
float4 Material_Texture2D_5_ST;
TEXTURE2D(       Material_Texture2D_6 );
SAMPLER(  samplerMaterial_Texture2D_6 );
float4 Material_Texture2D_6_TexelSize;
float4 Material_Texture2D_6_ST;
TEXTURE2D(       Material_Texture2D_7 );
SAMPLER(  samplerMaterial_Texture2D_7 );
float4 Material_Texture2D_7_TexelSize;
float4 Material_Texture2D_7_ST;
TEXTURE2D(       Material_Texture2D_8 );
SAMPLER(  samplerMaterial_Texture2D_8 );
float4 Material_Texture2D_8_TexelSize;
float4 Material_Texture2D_8_ST;

//};

#ifdef UE5
	#define UE_LWC_RENDER_TILE_SIZE			2097152.0
	#define UE_LWC_RENDER_TILE_SIZE_SQRT	1448.15466
	#define UE_LWC_RENDER_TILE_SIZE_RSQRT	0.000690533954
	#define UE_LWC_RENDER_TILE_SIZE_RCP		4.76837158e-07
	#define UE_LWC_RENDER_TILE_SIZE_FMOD_PI		0.673652053
	#define UE_LWC_RENDER_TILE_SIZE_FMOD_2PI	0.673652053
	#define INVARIANT(X) X
	#define PI 					(3.1415926535897932)

	#include "LargeWorldCoordinates.hlsl"
#endif
struct MaterialStruct
{
	float4 PreshaderBuffer[9];
	float4 ScalarExpressions[1];
	float VTPackedPageTableUniform[2];
	float VTPackedUniform[1];
};
static SamplerState View_MaterialTextureBilinearWrapedSampler;
static SamplerState View_MaterialTextureBilinearClampedSampler;
struct ViewStruct
{
	float GameTime;
	float RealTime;
	float DeltaTime;
	float PrevFrameGameTime;
	float PrevFrameRealTime;
	float MaterialTextureMipBias;	
	float4 PrimitiveSceneData[ 40 ];
	float4 TemporalAAParams;
	float2 ViewRectMin;
	float4 ViewSizeAndInvSize;
	float MaterialTextureDerivativeMultiply;
	uint StateFrameIndexMod8;
	float FrameNumber;
	float2 FieldOfViewWideAngles;
	float4 RuntimeVirtualTextureMipLevel;
	float PreExposure;
	float4 BufferBilinearUVMinMax;
};
struct ResolvedViewStruct
{
	#ifdef UE5
		FLWCVector3 WorldCameraOrigin;
		FLWCVector3 PrevWorldCameraOrigin;
		FLWCVector3 PreViewTranslation;
		FLWCVector3 WorldViewOrigin;
	#else
		float3 WorldCameraOrigin;
		float3 PrevWorldCameraOrigin;
		float3 PreViewTranslation;
		float3 WorldViewOrigin;
	#endif
	float4 ScreenPositionScaleBias;
	float4x4 TranslatedWorldToView;
	float4x4 TranslatedWorldToCameraView;
	float4x4 TranslatedWorldToClip;
	float4x4 ViewToTranslatedWorld;
	float4x4 PrevViewToTranslatedWorld;
	float4x4 CameraViewToTranslatedWorld;
	float4 BufferBilinearUVMinMax;
	float4 XRPassthroughCameraUVs[ 2 ];
};
struct PrimitiveStruct
{
	float4x4 WorldToLocal;
	float4x4 LocalToWorld;
};

static ViewStruct View;
static ResolvedViewStruct ResolvedView;
static PrimitiveStruct Primitive;
uniform float4 View_BufferSizeAndInvSize;
uniform float4 LocalObjectBoundsMin;
uniform float4 LocalObjectBoundsMax;
static SamplerState Material_Wrap_WorldGroupSettings;
static SamplerState Material_Clamp_WorldGroupSettings;

#include "UnrealCommon.cginc"

static MaterialStruct Material;
void InitializeExpressions()
{
	Material.PreshaderBuffer[0] = float4(1.000000,-0.000000,0.000000,1.000000);//(Unknown)
	Material.PreshaderBuffer[1] = float4(1.000000,1.000000,-0.000000,0.000000);//(Unknown)
	Material.PreshaderBuffer[2] = float4(0.000000,1.000000,1.000000,1.000000);//(Unknown)
	Material.PreshaderBuffer[3] = float4(0.000000,0.000000,0.000000,0.000000);//(Unknown)
	Material.PreshaderBuffer[4] = float4(0.000000,0.000000,0.000000,0.000000);//(Unknown)
	Material.PreshaderBuffer[5] = float4(1.000000,1.000000,0.619048,0.000000);//(Unknown)
	Material.PreshaderBuffer[6] = float4(1.000000,0.821039,0.453150,0.700000);//(Unknown)
	Material.PreshaderBuffer[7] = float4(1.000000,1.000000,1.000000,0.000000);//(Unknown)
	Material.PreshaderBuffer[8] = float4(32.164612,0.000000,1.000000,0.000000);//(Unknown)
}struct MaterialCollection0Type
{
	float4 Vectors[2];
};
//MPC_WeatherConditions
MaterialCollection0Type MaterialCollection0;
void Initialize_MaterialCollection0()
{
	MaterialCollection0.Vectors[0] = float4(0.000000,3.000000,50.000000,1.000000);//SnowCoverage,SnowTiling,SnowNormalIntensity,SnowHardness
	MaterialCollection0.Vectors[1] = float4(0.000000,0.000000,-0.030210,0.000000);//Season,SnowDisplacement,,
}
float3 GetMaterialWorldPositionOffset(FMaterialVertexParameters Parameters)
{
	MaterialFloat2 Local71 = Parameters.TexCoords[0].xy;
	MaterialFloat2 Local72 = (MaterialFloat2(-0.50000000,-0.50000000) + DERIV_BASE_VALUE(Local71));
	MaterialFloat Local73 = dot(DERIV_BASE_VALUE(Local72),Material.PreshaderBuffer[0].xy);
	MaterialFloat Local74 = dot(DERIV_BASE_VALUE(Local72),Material.PreshaderBuffer[0].zw);
	MaterialFloat2 Local75 = MaterialFloat2(DERIV_BASE_VALUE(Local73),DERIV_BASE_VALUE(Local74));
	MaterialFloat2 Local76 = (MaterialFloat2(0.50000000,0.50000000) + DERIV_BASE_VALUE(Local75));
	MaterialFloat2 Local77 = (DERIV_BASE_VALUE(Local76) * ((MaterialFloat2)Material.PreshaderBuffer[1].x));
	MaterialFloat4 Local78 = UnpackNormalMap(Texture2DSampleLevel(Material_Texture2D_0,samplerMaterial_Texture2D_0,DERIV_BASE_VALUE(Local77),-1.00000000));
	MaterialFloat Local79 = dot(DERIV_BASE_VALUE(Local72),Material.PreshaderBuffer[1].yz);
	MaterialFloat Local80 = dot(DERIV_BASE_VALUE(Local72),Material.PreshaderBuffer[2].xy);
	MaterialFloat2 Local81 = MaterialFloat2(DERIV_BASE_VALUE(Local79),DERIV_BASE_VALUE(Local80));
	MaterialFloat2 Local82 = (MaterialFloat2(0.50000000,0.50000000) + DERIV_BASE_VALUE(Local81));
	MaterialFloat2 Local83 = (DERIV_BASE_VALUE(Local82) * ((MaterialFloat2)Material.PreshaderBuffer[2].z));
	MaterialFloat4 Local84 = UnpackNormalMap(Texture2DSampleLevel(Material_Texture2D_1,samplerMaterial_Texture2D_1,DERIV_BASE_VALUE(Local83),-1.00000000));
	MaterialFloat4 Local85 = Parameters.VertexColor;
	MaterialFloat Local86 = DERIV_BASE_VALUE(Local85).r;
	MaterialFloat3 Local87 = lerp(Local78.rgb,Local84.rgb,DERIV_BASE_VALUE(Local86));
	MaterialFloat4 Local88 = MaterialCollection0.Vectors[0];
	MaterialFloat2 Local89 = (DERIV_BASE_VALUE(Local71) * ((MaterialFloat2)Local88.g));
	MaterialFloat4 Local90 = UnpackNormalMap(Texture2DSampleLevel(Material_Texture2D_2,samplerMaterial_Texture2D_2,DERIV_BASE_VALUE(Local89),-1.00000000));
	MaterialFloat3 Local91 = (Local90.rgb * ((MaterialFloat3)Local88.b));
	MaterialFloat Local92 = DERIV_BASE_VALUE(Local85).g;
	MaterialFloat3 Local93 = (Parameters.TangentToWorld[2] * ((MaterialFloat3)DERIV_BASE_VALUE(Local92)));
	MaterialFloat Local94 = (Local88.r - 1.00000000);
	MaterialFloat Local95 = (Local93.b + Local94);
	MaterialFloat Local96 = saturate(Local95);
	MaterialFloat3 Local97 = lerp(Local87,Local91,Local96);
	MaterialFloat3 Local98 = (Local97 * ((MaterialFloat3)Material.PreshaderBuffer[2].w));
	MaterialFloat4 Local99 = MaterialCollection0.Vectors[1];
	MaterialFloat3 Local100 = (Local98 * ((MaterialFloat3)Local99.g));
	return Local100;;
}
void CalcPixelMaterialInputs(in out FMaterialPixelParameters Parameters, in out FPixelMaterialInputs PixelMaterialInputs)
{
	//WorldAligned texturing & others use normals & stuff that think Z is up
	Parameters.TangentToWorld[0] = Parameters.TangentToWorld[0].xzy;
	Parameters.TangentToWorld[1] = Parameters.TangentToWorld[1].xzy;
	Parameters.TangentToWorld[2] = Parameters.TangentToWorld[2].xzy;

	float3 WorldNormalCopy = Parameters.WorldNormal;

	// Initial calculations (required for Normal)
	MaterialFloat2 Local0 = Parameters.TexCoords[0].xy;
	MaterialFloat2 Local1 = (MaterialFloat2(-0.50000000,-0.50000000) + DERIV_BASE_VALUE(Local0));
	MaterialFloat Local2 = dot(DERIV_BASE_VALUE(Local1),Material.PreshaderBuffer[0].xy);
	MaterialFloat Local3 = dot(DERIV_BASE_VALUE(Local1),Material.PreshaderBuffer[0].zw);
	MaterialFloat2 Local4 = MaterialFloat2(DERIV_BASE_VALUE(Local2),DERIV_BASE_VALUE(Local3));
	MaterialFloat2 Local5 = (MaterialFloat2(0.50000000,0.50000000) + DERIV_BASE_VALUE(Local4));
	MaterialFloat2 Local6 = (DERIV_BASE_VALUE(Local5) * ((MaterialFloat2)Material.PreshaderBuffer[1].x));
	MaterialFloat Local7 = MaterialStoreTexCoordScale(Parameters, DERIV_BASE_VALUE(Local6), 8);
	MaterialFloat4 Local8 = UnpackNormalMap(Texture2DSampleBias(Material_Texture2D_0,samplerMaterial_Texture2D_0,DERIV_BASE_VALUE(Local6),View.MaterialTextureMipBias));
	MaterialFloat Local9 = MaterialStoreTexSample(Parameters, Local8, 8);
	MaterialFloat Local10 = dot(DERIV_BASE_VALUE(Local1),Material.PreshaderBuffer[1].yz);
	MaterialFloat Local11 = dot(DERIV_BASE_VALUE(Local1),Material.PreshaderBuffer[2].xy);
	MaterialFloat2 Local12 = MaterialFloat2(DERIV_BASE_VALUE(Local10),DERIV_BASE_VALUE(Local11));
	MaterialFloat2 Local13 = (MaterialFloat2(0.50000000,0.50000000) + DERIV_BASE_VALUE(Local12));
	MaterialFloat2 Local14 = (DERIV_BASE_VALUE(Local13) * ((MaterialFloat2)Material.PreshaderBuffer[2].z));
	MaterialFloat Local15 = MaterialStoreTexCoordScale(Parameters, DERIV_BASE_VALUE(Local14), 2);
	MaterialFloat4 Local16 = UnpackNormalMap(Texture2DSampleBias(Material_Texture2D_1,samplerMaterial_Texture2D_1,DERIV_BASE_VALUE(Local14),View.MaterialTextureMipBias));
	MaterialFloat Local17 = MaterialStoreTexSample(Parameters, Local16, 2);
	MaterialFloat4 Local18 = Parameters.VertexColor;
	MaterialFloat Local19 = DERIV_BASE_VALUE(Local18).r;
	MaterialFloat3 Local20 = lerp(Local8.rgb,Local16.rgb,DERIV_BASE_VALUE(Local19));
	MaterialFloat4 Local21 = MaterialCollection0.Vectors[0];
	MaterialFloat2 Local22 = (DERIV_BASE_VALUE(Local0) * ((MaterialFloat2)Local21.g));
	MaterialFloat Local23 = MaterialStoreTexCoordScale(Parameters, DERIV_BASE_VALUE(Local22), 9);
	MaterialFloat4 Local24 = UnpackNormalMap(Texture2DSampleBias(Material_Texture2D_2,samplerMaterial_Texture2D_2,DERIV_BASE_VALUE(Local22),View.MaterialTextureMipBias));
	MaterialFloat Local25 = MaterialStoreTexSample(Parameters, Local24, 9);
	MaterialFloat3 Local26 = (Local24.rgb * ((MaterialFloat3)Local21.b));
	MaterialFloat Local27 = DERIV_BASE_VALUE(Local18).g;
	MaterialFloat3 Local28 = (Parameters.TangentToWorld[2] * ((MaterialFloat3)DERIV_BASE_VALUE(Local27)));
	MaterialFloat Local29 = (Local21.r - 1.00000000);
	MaterialFloat Local30 = (Local28.b + Local29);
	MaterialFloat Local31 = saturate(Local30);
	MaterialFloat3 Local32 = lerp(Local20,Local26,Local31);
	MaterialFloat3 Local33 = (Local32 * ((MaterialFloat3)Material.PreshaderBuffer[2].w));

	// The Normal is a special case as it might have its own expressions and also be used to calculate other inputs, so perform the assignment here
	PixelMaterialInputs.Normal = Local33;


#if TEMPLATE_USES_SUBSTRATE
	Parameters.SubstratePixelFootprint = SubstrateGetPixelFootprint(Parameters.WorldPosition_CamRelative, GetRoughnessFromNormalCurvature(Parameters));
	Parameters.SharedLocalBases = SubstrateInitialiseSharedLocalBases();
	Parameters.SubstrateTree = GetInitialisedSubstrateTree();
#if SUBSTRATE_USE_FULLYSIMPLIFIED_MATERIAL == 1
	Parameters.SharedLocalBasesFullySimplified = SubstrateInitialiseSharedLocalBases();
	Parameters.SubstrateTreeFullySimplified = GetInitialisedSubstrateTree();
#endif
#endif

	// Note that here MaterialNormal can be in world space or tangent space
	float3 MaterialNormal = GetMaterialNormal(Parameters, PixelMaterialInputs);

#if MATERIAL_TANGENTSPACENORMAL

#if FEATURE_LEVEL >= FEATURE_LEVEL_SM4
	// Mobile will rely on only the final normalize for performance
	MaterialNormal = normalize(MaterialNormal);
#endif

	// normalizing after the tangent space to world space conversion improves quality with sheared bases (UV layout to WS causes shrearing)
	// use full precision normalize to avoid overflows
	Parameters.WorldNormal = TransformTangentNormalToWorld(Parameters.TangentToWorld, MaterialNormal);

#else //MATERIAL_TANGENTSPACENORMAL

	Parameters.WorldNormal = normalize(MaterialNormal);

#endif //MATERIAL_TANGENTSPACENORMAL

#if MATERIAL_TANGENTSPACENORMAL || TWO_SIDED_WORLD_SPACE_SINGLELAYERWATER_NORMAL
	// flip the normal for backfaces being rendered with a two-sided material
	Parameters.WorldNormal *= Parameters.TwoSidedSign;
#endif

	Parameters.ReflectionVector = ReflectionAboutCustomWorldNormal(Parameters, Parameters.WorldNormal, false);

#if !PARTICLE_SPRITE_FACTORY
	Parameters.Particle.MotionBlurFade = 1.0f;
#endif // !PARTICLE_SPRITE_FACTORY

	// Now the rest of the inputs
	MaterialFloat Local34 = MaterialStoreTexCoordScale(Parameters, DERIV_BASE_VALUE(Local0), 3);
	MaterialFloat4 Local35 = ProcessMaterialColorTextureLookup(Texture2DSampleBias(Material_Texture2D_3,samplerMaterial_Texture2D_3,DERIV_BASE_VALUE(Local0),View.MaterialTextureMipBias));
	MaterialFloat Local36 = MaterialStoreTexSample(Parameters, Local35, 3);
	MaterialFloat3 Local37 = (Local35.rgb * Material.PreshaderBuffer[3].xyz);
	MaterialFloat3 Local38 = (Local37 * ((MaterialFloat3)Material.PreshaderBuffer[3].w));
	MaterialFloat3 Local39 = lerp(Local38,Material.PreshaderBuffer[4].yzw,Material.PreshaderBuffer[4].x);
	MaterialFloat2 Local40 = (DERIV_BASE_VALUE(Local5) * ((MaterialFloat2)Material.PreshaderBuffer[5].x));
	MaterialFloat Local41 = MaterialStoreTexCoordScale(Parameters, DERIV_BASE_VALUE(Local40), 6);
	MaterialFloat4 Local42 = ProcessMaterialColorTextureLookup(Texture2DSampleBias(Material_Texture2D_4,samplerMaterial_Texture2D_4,DERIV_BASE_VALUE(Local40),View.MaterialTextureMipBias));
	MaterialFloat Local43 = MaterialStoreTexSample(Parameters, Local42, 6);
	MaterialFloat2 Local44 = (DERIV_BASE_VALUE(Local13) * ((MaterialFloat2)Material.PreshaderBuffer[5].y));
	MaterialFloat Local45 = MaterialStoreTexCoordScale(Parameters, DERIV_BASE_VALUE(Local44), 1);
	MaterialFloat4 Local46 = ProcessMaterialColorTextureLookup(Texture2DSampleBias(Material_Texture2D_5,samplerMaterial_Texture2D_5,DERIV_BASE_VALUE(Local44),View.MaterialTextureMipBias));
	MaterialFloat Local47 = MaterialStoreTexSample(Parameters, Local46, 1);
	MaterialFloat3 Local48 = lerp(Local42.rgb,Local46.rgb,DERIV_BASE_VALUE(Local19));
	MaterialFloat3 Local49 = (Local48 * ((MaterialFloat3)Material.PreshaderBuffer[5].z));
	MaterialFloat3 Local50 = (Local49 * Material.PreshaderBuffer[6].xyz);
	MaterialFloat Local51 = dot(Local50,MaterialFloat3(0.30000001,0.58999997,0.11000000));
	MaterialFloat3 Local52 = lerp(Local50,((MaterialFloat3)Local51),Material.PreshaderBuffer[6].w);
	MaterialFloat3 Local53 = lerp(Local52,((MaterialFloat3)1.00000000),Local31);
	MaterialFloat2 Local54 = (DERIV_BASE_VALUE(Local5) * ((MaterialFloat2)Material.PreshaderBuffer[7].x));
	MaterialFloat Local55 = MaterialStoreTexCoordScale(Parameters, DERIV_BASE_VALUE(Local54), 7);
	MaterialFloat4 Local56 = ProcessMaterialColorTextureLookup(Texture2DSampleBias(Material_Texture2D_6,samplerMaterial_Texture2D_6,DERIV_BASE_VALUE(Local54),View.MaterialTextureMipBias));
	MaterialFloat Local57 = MaterialStoreTexSample(Parameters, Local56, 7);
	MaterialFloat2 Local58 = (DERIV_BASE_VALUE(Local13) * ((MaterialFloat2)Material.PreshaderBuffer[7].y));
	MaterialFloat Local59 = MaterialStoreTexCoordScale(Parameters, DERIV_BASE_VALUE(Local58), 0);
	MaterialFloat4 Local60 = ProcessMaterialColorTextureLookup(Texture2DSampleBias(Material_Texture2D_7,samplerMaterial_Texture2D_7,DERIV_BASE_VALUE(Local58),View.MaterialTextureMipBias));
	MaterialFloat Local61 = MaterialStoreTexSample(Parameters, Local60, 0);
	MaterialFloat Local62 = lerp(Local56.g,Local60.g,DERIV_BASE_VALUE(Local19));
	MaterialFloat Local63 = (Local62 * Material.PreshaderBuffer[7].z);
	MaterialFloat Local64 = lerp(Local56.r,Local60.r,DERIV_BASE_VALUE(Local19));
	MaterialFloat Local65 = lerp(Material.PreshaderBuffer[8].y,Material.PreshaderBuffer[8].x,DERIV_BASE_VALUE(Local19));
	MaterialFloat Local66 = (Local64 * DERIV_BASE_VALUE(Local65));
	MaterialFloat2 Local67 = (DERIV_BASE_VALUE(Local13) * ((MaterialFloat2)Material.PreshaderBuffer[8].z));
	MaterialFloat Local68 = MaterialStoreTexCoordScale(Parameters, DERIV_BASE_VALUE(Local67), 5);
	MaterialFloat4 Local69 = ProcessMaterialColorTextureLookup(Texture2DSampleBias(Material_Texture2D_8,samplerMaterial_Texture2D_8,DERIV_BASE_VALUE(Local67),View.MaterialTextureMipBias));
	MaterialFloat Local70 = MaterialStoreTexSample(Parameters, Local69, 5);
	MaterialFloat Local101 = lerp(Local56.b,Local60.b,DERIV_BASE_VALUE(Local19));

	PixelMaterialInputs.EmissiveColor = Local39;
	PixelMaterialInputs.Opacity = 1.00000000;
	PixelMaterialInputs.OpacityMask = Local69.rgb;
	PixelMaterialInputs.BaseColor = Local53;
	PixelMaterialInputs.Metallic = Local63;
	PixelMaterialInputs.Specular = Material.PreshaderBuffer[7].w;
	PixelMaterialInputs.Roughness = Local66;
	PixelMaterialInputs.Anisotropy = 0.00000000;
	PixelMaterialInputs.Normal = Local33;
	PixelMaterialInputs.Tangent = MaterialFloat3(1.00000000,0.00000000,0.00000000);
	PixelMaterialInputs.Subsurface = 0;
	PixelMaterialInputs.AmbientOcclusion = Local101;
	PixelMaterialInputs.Refraction = 0;
	PixelMaterialInputs.PixelDepthOffset = 0.00000000;
	PixelMaterialInputs.ShadingModel = 1;
	PixelMaterialInputs.FrontMaterial = GetInitialisedSubstrateData();
	PixelMaterialInputs.SurfaceThickness = 0.01000000;
	PixelMaterialInputs.Displacement = 0.50000000;


#if MATERIAL_USES_ANISOTROPY
	Parameters.WorldTangent = CalculateAnisotropyTangent(Parameters, PixelMaterialInputs);
#else
	Parameters.WorldTangent = 0;
#endif
}

#define UnityObjectToWorldDir TransformObjectToWorld

void SetupCommonData( int Parameters_PrimitiveId )
{
	View_MaterialTextureBilinearWrapedSampler = SamplerState_Linear_Repeat;
	View_MaterialTextureBilinearClampedSampler = SamplerState_Linear_Clamp;

	Material_Wrap_WorldGroupSettings = SamplerState_Linear_Repeat;
	Material_Clamp_WorldGroupSettings = SamplerState_Linear_Clamp;

	View.GameTime = View.RealTime = _Time.y;// _Time is (t/20, t, t*2, t*3)
	View.PrevFrameGameTime = View.GameTime - unity_DeltaTime.x;//(dt, 1/dt, smoothDt, 1/smoothDt)
	View.PrevFrameRealTime = View.RealTime;
	View.DeltaTime = unity_DeltaTime.x;
	View.MaterialTextureMipBias = 0.0;
	View.TemporalAAParams = float4( 0, 0, 0, 0 );
	View.ViewRectMin = float2( 0, 0 );
	View.ViewSizeAndInvSize = View_BufferSizeAndInvSize;
	View.MaterialTextureDerivativeMultiply = 1.0f;
	View.StateFrameIndexMod8 = 0;
	View.FrameNumber = (int)_Time.y;
	View.FieldOfViewWideAngles = float2( PI * 0.42f, PI * 0.42f );//75degrees, default unity
	View.RuntimeVirtualTextureMipLevel = float4( 0, 0, 0, 0 );
	View.PreExposure = 0;
	View.BufferBilinearUVMinMax = float4(
		View_BufferSizeAndInvSize.z * ( 0 + 0.5 ),//EffectiveViewRect.Min.X
		View_BufferSizeAndInvSize.w * ( 0 + 0.5 ),//EffectiveViewRect.Min.Y
		View_BufferSizeAndInvSize.z * ( View_BufferSizeAndInvSize.x - 0.5 ),//EffectiveViewRect.Max.X
		View_BufferSizeAndInvSize.w * ( View_BufferSizeAndInvSize.y - 0.5 ) );//EffectiveViewRect.Max.Y

	for( int i2 = 0; i2 < 40; i2++ )
		View.PrimitiveSceneData[ i2 ] = float4( 0, 0, 0, 0 );

	float4x4 LocalToWorld = transpose( UNITY_MATRIX_M );
    LocalToWorld[3] = float4(ToUnrealPos(LocalToWorld[3]), LocalToWorld[3].w);
	float4x4 WorldToLocal = transpose( UNITY_MATRIX_I_M );
	float4x4 ViewMatrix = transpose( UNITY_MATRIX_V );
	float4x4 InverseViewMatrix = transpose( UNITY_MATRIX_I_V );
	float4x4 ViewProjectionMatrix = transpose( UNITY_MATRIX_VP );
	uint PrimitiveBaseOffset = Parameters_PrimitiveId * PRIMITIVE_SCENE_DATA_STRIDE;
	View.PrimitiveSceneData[ PrimitiveBaseOffset + 0 ] = LocalToWorld[ 0 ];//LocalToWorld
	View.PrimitiveSceneData[ PrimitiveBaseOffset + 1 ] = LocalToWorld[ 1 ];//LocalToWorld
	View.PrimitiveSceneData[ PrimitiveBaseOffset + 2 ] = LocalToWorld[ 2 ];//LocalToWorld
	View.PrimitiveSceneData[ PrimitiveBaseOffset + 3 ] = LocalToWorld[ 3 ];//LocalToWorld
	View.PrimitiveSceneData[ PrimitiveBaseOffset + 5 ] = float4( ToUnrealPos( SHADERGRAPH_OBJECT_POSITION ), 100.0 );//ObjectWorldPosition
	View.PrimitiveSceneData[ PrimitiveBaseOffset + 6 ] = WorldToLocal[ 0 ];//WorldToLocal
	View.PrimitiveSceneData[ PrimitiveBaseOffset + 7 ] = WorldToLocal[ 1 ];//WorldToLocal
	View.PrimitiveSceneData[ PrimitiveBaseOffset + 8 ] = WorldToLocal[ 2 ];//WorldToLocal
	View.PrimitiveSceneData[ PrimitiveBaseOffset + 9 ] = WorldToLocal[ 3 ];//WorldToLocal
	View.PrimitiveSceneData[ PrimitiveBaseOffset + 10 ] = LocalToWorld[ 0 ];//PreviousLocalToWorld
	View.PrimitiveSceneData[ PrimitiveBaseOffset + 11 ] = LocalToWorld[ 1 ];//PreviousLocalToWorld
	View.PrimitiveSceneData[ PrimitiveBaseOffset + 12 ] = LocalToWorld[ 2 ];//PreviousLocalToWorld
	View.PrimitiveSceneData[ PrimitiveBaseOffset + 13 ] = LocalToWorld[ 3 ];//PreviousLocalToWorld
	View.PrimitiveSceneData[ PrimitiveBaseOffset + 18 ] = float4( ToUnrealPos( SHADERGRAPH_OBJECT_POSITION ), 0 );//ActorWorldPosition
	View.PrimitiveSceneData[ PrimitiveBaseOffset + 19 ] = LocalObjectBoundsMax - LocalObjectBoundsMin;//ObjectBounds
	View.PrimitiveSceneData[ PrimitiveBaseOffset + 21 ] = mul( LocalToWorld, float3( 1, 0, 0 ) );
	View.PrimitiveSceneData[ PrimitiveBaseOffset + 23 ] = LocalObjectBoundsMin;//LocalObjectBoundsMin 
	View.PrimitiveSceneData[ PrimitiveBaseOffset + 24 ] = LocalObjectBoundsMax;//LocalObjectBoundsMax

#ifdef UE5
	ResolvedView.WorldCameraOrigin = LWCPromote( ToUnrealPos( _WorldSpaceCameraPos.xyz ) );
	ResolvedView.PreViewTranslation = LWCPromote( float3( 0, 0, 0 ) );
	ResolvedView.WorldViewOrigin = LWCPromote( float3( 0, 0, 0 ) );
#else
	ResolvedView.WorldCameraOrigin = ToUnrealPos( _WorldSpaceCameraPos.xyz );
	ResolvedView.PreViewTranslation = float3( 0, 0, 0 );
	ResolvedView.WorldViewOrigin = float3( 0, 0, 0 );
#endif
	ResolvedView.PrevWorldCameraOrigin = ResolvedView.WorldCameraOrigin;
	ResolvedView.ScreenPositionScaleBias = float4( 1, 1, 0, 0 );
	ResolvedView.TranslatedWorldToView		 = ViewMatrix;
	ResolvedView.TranslatedWorldToCameraView = ViewMatrix;
	ResolvedView.TranslatedWorldToClip		 = ViewProjectionMatrix;
	ResolvedView.ViewToTranslatedWorld		 = InverseViewMatrix;
	ResolvedView.PrevViewToTranslatedWorld = ResolvedView.ViewToTranslatedWorld;
	ResolvedView.CameraViewToTranslatedWorld = InverseViewMatrix;
	ResolvedView.BufferBilinearUVMinMax = View.BufferBilinearUVMinMax;
	Primitive.WorldToLocal = WorldToLocal;
	Primitive.LocalToWorld = LocalToWorld;
}
#define VS_USES_UNREAL_SPACE 1
float3 PrepareAndGetWPO( float4 VertexColor, float3 UnrealWorldPos, float3 UnrealNormal, float4 InTangent,
						 float4 UV0, float4 UV1 )
{
	InitializeExpressions();
	Initialize_MaterialCollection0();

	FMaterialVertexParameters Parameters = (FMaterialVertexParameters)0;

	float3 InWorldNormal = UnrealNormal;
	float4 tangentWorld = InTangent;
	tangentWorld.xyz = normalize( tangentWorld.xyz );
	//float3x3 tangentToWorld = CreateTangentToWorldPerVertex( InWorldNormal, tangentWorld.xyz, tangentWorld.w );
	Parameters.TangentToWorld = float3x3( normalize( cross( InWorldNormal, tangentWorld.xyz ) * tangentWorld.w ), tangentWorld.xyz, InWorldNormal );

	
	#ifdef VS_USES_UNREAL_SPACE
		UnrealWorldPos = ToUnrealPos( UnrealWorldPos );
	#endif
	Parameters.WorldPosition = UnrealWorldPos;
	#ifdef VS_USES_UNREAL_SPACE
		Parameters.TangentToWorld[ 0 ] = Parameters.TangentToWorld[ 0 ].xzy;
		Parameters.TangentToWorld[ 1 ] = Parameters.TangentToWorld[ 1 ].xzy;
		Parameters.TangentToWorld[ 2 ] = Parameters.TangentToWorld[ 2 ].xzy;//WorldAligned texturing uses normals that think Z is up
	#endif

	Parameters.VertexColor = VertexColor;

#if NUM_MATERIAL_TEXCOORDS_VERTEX > 0			
	Parameters.TexCoords[ 0 ] = float2( UV0.x, UV0.y );
#endif
#if NUM_MATERIAL_TEXCOORDS_VERTEX > 1
	Parameters.TexCoords[ 1 ] = float2( UV1.x, UV1.y );
#endif
#if NUM_MATERIAL_TEXCOORDS_VERTEX > 2
	for( int i = 2; i < NUM_TEX_COORD_INTERPOLATORS; i++ )
	{
		Parameters.TexCoords[ i ] = float2( UV0.x, UV0.y );
	}
#endif

	Parameters.PrimitiveId = 0;

	SetupCommonData( Parameters.PrimitiveId );

#ifdef UE5
	Parameters.PrevFrameLocalToWorld = MakeLWCMatrix( float3( 0, 0, 0 ), Primitive.LocalToWorld );
#else
	Parameters.PrevFrameLocalToWorld = Primitive.LocalToWorld;
#endif
	
	float3 Offset = float3( 0, 0, 0 );
	Offset = GetMaterialWorldPositionOffset( Parameters );
	#ifdef VS_USES_UNREAL_SPACE
		//Convert from unreal units to unity
		Offset /= float3( 100, 100, 100 );
		Offset = Offset.xzy;
	#endif
	return Offset;
}

void SurfaceReplacement( Input In, out SurfaceOutputStandard o )
{
	InitializeExpressions();
	Initialize_MaterialCollection0();


	float3 Z3 = float3( 0, 0, 0 );
	float4 Z4 = float4( 0, 0, 0, 0 );

	float3 UnrealWorldPos = float3( In.worldPos.x, In.worldPos.y, In.worldPos.z );

	float3 UnrealNormal = In.normal2;	

	FMaterialPixelParameters Parameters = (FMaterialPixelParameters)0;
#if NUM_TEX_COORD_INTERPOLATORS > 0			
	Parameters.TexCoords[ 0 ] = float2( In.uv_MainTex.x, 1.0 - In.uv_MainTex.y );
#endif
#if NUM_TEX_COORD_INTERPOLATORS > 1
	Parameters.TexCoords[ 1 ] = float2( In.uv2_Material_Texture2D_0.x, 1.0 - In.uv2_Material_Texture2D_0.y );
#endif
#if NUM_TEX_COORD_INTERPOLATORS > 2
	for( int i = 2; i < NUM_TEX_COORD_INTERPOLATORS; i++ )
	{
		Parameters.TexCoords[ i ] = float2( In.uv_MainTex.x, 1.0 - In.uv_MainTex.y );
	}
#endif
	Parameters.VertexColor = In.color;
	Parameters.WorldNormal = UnrealNormal;
	Parameters.ReflectionVector = half3( 0, 0, 1 );
	Parameters.CameraVector = normalize( _WorldSpaceCameraPos.xyz - UnrealWorldPos.xyz );
	//Parameters.CameraVector = mul( ( float3x3 )unity_CameraToWorld, float3( 0, 0, 1 ) ) * -1;
	Parameters.LightVector = half3( 0, 0, 0 );
	//float4 screenpos = In.screenPos;
	//screenpos /= screenpos.w;
	Parameters.SvPosition = In.screenPos;
	Parameters.ScreenPosition = Parameters.SvPosition;

	Parameters.UnMirrored = 1;

	Parameters.TwoSidedSign = 1;


	float3 InWorldNormal = UnrealNormal;	
	float4 tangentWorld = In.tangent;
	tangentWorld.xyz = normalize( tangentWorld.xyz );
	//float3x3 tangentToWorld = CreateTangentToWorldPerVertex( InWorldNormal, tangentWorld.xyz, tangentWorld.w );
	Parameters.TangentToWorld = float3x3( normalize( cross( InWorldNormal, tangentWorld.xyz ) * tangentWorld.w ), tangentWorld.xyz, InWorldNormal );

	//WorldAlignedTexturing in UE relies on the fact that coords there are 100x larger, prepare values for that
	//but watch out for any computation that might get skewed as a side effect
	UnrealWorldPos = ToUnrealPos( UnrealWorldPos );
	
	Parameters.AbsoluteWorldPosition = UnrealWorldPos;
	Parameters.WorldPosition_CamRelative = UnrealWorldPos;
	Parameters.WorldPosition_NoOffsets = UnrealWorldPos;

	Parameters.WorldPosition_NoOffsets_CamRelative = Parameters.WorldPosition_CamRelative;
	Parameters.LightingPositionOffset = float3( 0, 0, 0 );

	Parameters.AOMaterialMask = 0;

	Parameters.Particle.RelativeTime = 0;
	Parameters.Particle.MotionBlurFade;
	Parameters.Particle.Random = 0;
	Parameters.Particle.Velocity = half4( 1, 1, 1, 1 );
	Parameters.Particle.Color = half4( 1, 1, 1, 1 );
	Parameters.Particle.TranslatedWorldPositionAndSize = float4( UnrealWorldPos, 0 );
	Parameters.Particle.MacroUV = half4( 0, 0, 1, 1 );
	Parameters.Particle.DynamicParameter = half4( 0, 0, 0, 0 );
	Parameters.Particle.LocalToWorld = float4x4( Z4, Z4, Z4, Z4 );
	Parameters.Particle.Size = float2( 1, 1 );
	Parameters.Particle.SubUVCoords[ 0 ] = Parameters.Particle.SubUVCoords[ 1 ] = float2( 0, 0 );
	Parameters.Particle.SubUVLerp = 0.0;
	Parameters.TexCoordScalesParams = float2( 0, 0 );
	Parameters.PrimitiveId = 0;
	Parameters.VirtualTextureFeedback = 0;

	FPixelMaterialInputs PixelMaterialInputs = (FPixelMaterialInputs)0;
	PixelMaterialInputs.Normal = float3( 0, 0, 1 );
	PixelMaterialInputs.ShadingModel = 0;
	PixelMaterialInputs.FrontMaterial = 0;

	SetupCommonData( Parameters.PrimitiveId );
	//CustomizedUVs
	#if NUM_TEX_COORD_INTERPOLATORS > 0 && HAS_CUSTOMIZED_UVS
		float2 OutTexCoords[ NUM_TEX_COORD_INTERPOLATORS ];
		//Prevent uninitialized reads
		for( int i = 0; i < NUM_TEX_COORD_INTERPOLATORS; i++ )
		{
			OutTexCoords[ i ] = float2( 0, 0 );
		}
		GetMaterialCustomizedUVs( Parameters, OutTexCoords );
		for( int i = 0; i < NUM_TEX_COORD_INTERPOLATORS; i++ )
		{
			Parameters.TexCoords[ i ] = OutTexCoords[ i ];
		}
	#endif
	//<-
	CalcPixelMaterialInputs( Parameters, PixelMaterialInputs );

	#define HAS_WORLDSPACE_NORMAL 0
	#if HAS_WORLDSPACE_NORMAL
		PixelMaterialInputs.Normal = mul( PixelMaterialInputs.Normal, (MaterialFloat3x3)( transpose( Parameters.TangentToWorld ) ) );
	#endif

	o.Albedo = PixelMaterialInputs.BaseColor.rgb;
	o.Alpha = PixelMaterialInputs.Opacity;
	if( PixelMaterialInputs.OpacityMask < 0.333 ) discard;

	o.Metallic = PixelMaterialInputs.Metallic;
	o.Smoothness = 1.0 - PixelMaterialInputs.Roughness;
	o.Normal = normalize( PixelMaterialInputs.Normal );
	o.Emission = PixelMaterialInputs.EmissiveColor.rgb;
	o.Occlusion = PixelMaterialInputs.AmbientOcclusion;

	//BLEND_ADDITIVE o.Alpha = ( o.Emission.r + o.Emission.g + o.Emission.b ) / 3;
}