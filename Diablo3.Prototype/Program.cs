using System.Diagnostics;
using Diablo3.Api.Core;
using Diablo3.Api.Core.Models;

var configuration = new ClientConfiguration(new CacheConfiguration(CacheOptions.Default, 2000));
var client = new DiabloClientFactory(Region.EU, "d0696d1b62c940d3ace557ddd33eed54", "j9Z13V9a5W555ltePARV15pgGh8am3Z1")
    .Build(configuration);

var watch = new Stopwatch();

// watch.Start();
// var data = await client.GetForClassAsync(PlayerClass.Barbarian);
// watch.Stop();
// Console.WriteLine($"Barbarian leaderboard loaded in {watch.ElapsedMilliseconds / 1000}s");
// foreach (var entry in data.Entries)
//     Console.WriteLine(entry.RiftInformation.Level + ": " + entry.Player.Name + " - " + entry.RiftInformation.ItemSet);

watch.Restart();
var data4 = await client.GetAllAsync();
watch.Stop();
Console.WriteLine($"All leaderboards loaded in {watch.ElapsedMilliseconds / 1000}s");
Console.WriteLine($"Total leaderboard entries loaded: {data4.SelectMany(d => d.Entries).ToList().Count}");

var orderedData = data4.SelectMany(d => d.Entries).OrderByDescending(e => e.RiftInformation.Level).ToList();
var highestGR = orderedData.First();
Console.WriteLine($"Highest GR: {highestGR.RiftInformation.Level} - {highestGR.Player.PlayerClass} - {highestGR.Player.Name}");
for (var i = 0; i < 100; i++)
{
    Console.WriteLine($"{i}: {orderedData[i].Player.Name}, {orderedData[i].RiftInformation.Level}, {orderedData[i].Player.PlayerClass}, {orderedData[i].RiftInformation.ItemSet}");
}
//
// foreach (var d in data.Entries)
// {
//     Console.WriteLine($"{d.Player.PlayerClass}, {d.RiftInformation.Level}, {d.RiftInformation.ItemSet}");
// }