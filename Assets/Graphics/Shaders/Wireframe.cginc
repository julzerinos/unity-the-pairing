fixed4 _LineColor;
float _LineSize;
float4 _EmissionColor;
float _Intencity;
 
 
struct appdata
{
    float4 vertex : POSITION;
    float3 normal : NORMAL;
    float2 uv : TEXCOORD0;
};
 
struct v2f
{
    float2 uv : TEXCOORD0;
    float4 vertex : SV_POSITION;
    float3 normal : TEXCOORD1;
};
 
v2f vert (appdata v)
{
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    //If we are rendering in shaded mode (showing the original mesh renderer)
    //we want to ensure that the wireframe-processed mesh appears "on top" of
    //the original mesh. We achieve this by slightly decreasing the z component
    //(making the vertex closer to the camera) without actually changing its screen space position
    //since the w component remains the same, and thus, after w division, the x and y components
    //won't be affected by our "trick".
    //So, in essence, this just changes the value that gets written to the Z-Buffer
    o.vertex.z -= 0.001;
    o.uv = v.uv;
    o.normal = v.normal;
    return o;
}
 
 
fixed4 frag (v2f i) : SV_Target
{
    float lineWidthInPixels = _LineSize;
    float lineAntiaAliasWidthInPixels = 1;
 
 
    float2 normalxVector = float2(ddx(i.normal.x),ddy(i.normal.x)); 
    float2 normalyVector = float2(ddx(i.normal.y),ddy(i.normal.y)); 
    float2 normalzVector = float2(ddx(i.normal.z),ddy(i.normal.z)); 
 
    float normalxLength = length(normalxVector); 
    float normalyLength = length(normalyVector);
    float normalzLength = length(normalzVector); 
 
 
    float maximumXDistance = lineWidthInPixels * normalxLength;
    float maximumYDistance = lineWidthInPixels * normalyLength;
    float maximumZDistance = lineWidthInPixels * normalzLength;
 
    float minimumXDistance = i.normal.x;
    float minimumYDistance = i.normal.y;
    float minimumZDistance = i.normal.z;
 
 
 
    float normalizedXDistance = minimumXDistance / maximumXDistance;
    float normalizedYDistance = minimumYDistance / maximumYDistance;
    float normalizedZDistance = minimumZDistance / maximumZDistance;
 
 
 
    float closestNormalizedDistance = min(normalizedXDistance,normalizedYDistance);
    closestNormalizedDistance = min(closestNormalizedDistance,normalizedZDistance);
 
 
    float lineAlpha = 1.0 - smoothstep(1.0,1.0 + (lineAntiaAliasWidthInPixels/lineWidthInPixels),closestNormalizedDistance);
 
    lineAlpha *= _LineColor.a;
 
    
    return fixed4(_LineColor.rgb + _EmissionColor.rgb * _Intencity,lineAlpha);
 
}