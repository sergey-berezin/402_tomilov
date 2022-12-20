using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using ArcFaceNuget;

MyArcFace MyFace = new();
var face1 = Image.Load<Rgb24>("face1.png");
var face2 = Image.Load<Rgb24>("face2.png");
CancellationTokenSource cts = new CancellationTokenSource();
CancellationToken ct = cts.Token;
var dist = await MyFace.Distance(face1, face2, ct);
var sim = await MyFace.Similarity(face1, face2, ct);
//var dist = await MyFace.Distance(face1, face1, ct);
//var sim = await MyFace.Similarity(face1, face1, ct);
Console.WriteLine(dist);
Console.WriteLine(sim);