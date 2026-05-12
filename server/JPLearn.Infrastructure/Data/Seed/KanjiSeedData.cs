using JPLearn.Core.Kanji.Entities;
using Microsoft.EntityFrameworkCore;

namespace JPLearn.Infrastructure.Data.Seed;

public static class KanjiSeedData
{
    private static readonly DateTime SeededAt = new(2026, 5, 12, 0, 0, 0, DateTimeKind.Utc);

    public static async Task SeedAsync(AppDbContext db)
    {
        await ClearExistingDataAsync(db);
        await SeedLessonsAsync(db);
        await db.SaveChangesAsync();
    }

    private static async Task ClearExistingDataAsync(AppDbContext db)
    {
        db.UserKanjiProgress.RemoveRange(db.UserKanjiProgress);
        db.KanjiVocabularyItems.RemoveRange(db.KanjiVocabularyItems);
        db.KanjiItems.RemoveRange(db.KanjiItems);
        db.KanjiLessons.RemoveRange(db.KanjiLessons);
        await db.SaveChangesAsync();
    }

    private static async Task SeedLessonsAsync(AppDbContext db)
    {
        // Lesson 1: Giới thiệu bản thân và Trường học (JPD113)
        var lesson1Id = Guid.Parse("c051212d-48c6-5be0-a941-26505615cb23");
        if (!await db.KanjiLessons.AnyAsync(l => l.Id == lesson1Id))
        {
            db.KanjiLessons.Add(new KanjiLesson { Id = lesson1Id, Level = "N5", LessonNumber = 1, Title = "Giới thiệu bản thân và Trường học", Description = "Giới thiệu bản thân và Trường học - JPD113 Kanji N5.", AccessTier = "free", PackageCode = "kanji_jpd113", OrderIndex = 1, CreatedAt = SeededAt, UpdatedAt = SeededAt });
        }

        var kanjiItems1 = new List<KanjiItem>
        {
            new KanjiItem {
                Id = Guid.Parse("28bf8b0c-7514-5252-b44f-4cae34d664a9"),
                LessonId = lesson1Id,
                Level = "N5",
                Character = "私",
                HanViet = "TƯ",
                Meaning = "Tôi, riêng tư",
                StrokeCount = 7,
                KunReading = "わたし、わたくし",
                OnReading = "シ",
                Mnemonic = "Giữ bó lúa (禾) cho riêng mình (厶) tạo thành chữ tôi (私).",
                ComponentMapJson = @"[{""character"": ""禾"", ""component"": ""禾"", ""name"": ""lúa"", ""meaning"": ""lúa""}, {""character"": ""厶"", ""component"": ""厶"", ""name"": ""riêng tư"", ""meaning"": ""riêng tư""}]",
                StrokeDataJson = @"[""M45.24,13.5c0.01,1-0.5,2.25-1.43,2.99C39.75,19.75,31.75,23.88,18.75,29"", ""M11.62,44.6c2.63,0.78,5.51,0.5,7.28,0.17c9.98-1.89,22.62-3.29,30.72-4.06c1.29-0.12,3.13-0.21,4.36,0.12"", ""M36.08,26.66c0.95,0.95,1.41,2.84,1.41,4.69c0,3.83,0,40.18-0.04,55.4c-0.01,3.25-0.02,5.59-0.03,6.5"", ""M35.93,43.47c0,1.03-0.71,2.97-1.36,4.2C29.27,57.83,21.15,69.5,10.5,77.5"", ""M41,49.75c4.4,2.51,8.13,7.52,10.5,10.75"", ""M73.3,31.9c0.32,1.22,0.34,2.31-0.06,3.51C68.25,50.5,59.75,65.25,52.08,78.31c-2.12,3.61-1.6,5.03,1.88,3.89C66,78.25,76.62,74.12,88.28,69.98"", ""M82.81,60.54c4.45,4.37,11.49,17.97,12.61,24.77""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_079c1"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:079c1"" kvg:element=""私"">
	<g id=""kvg:079c1-g1"" kvg:element=""禾"" kvg:position=""left"" kvg:radical=""general"">
		<g id=""kvg:079c1-g2"" kvg:element=""丿"" kvg:position=""top"">
			<path id=""kvg:079c1-s1"" kvg:type=""㇒"" d=""M45.24,13.5c0.01,1-0.5,2.25-1.43,2.99C39.75,19.75,31.75,23.88,18.75,29""/>
		</g>
		<g id=""kvg:079c1-g3"" kvg:element=""木"" kvg:position=""bottom"">
			<path id=""kvg:079c1-s2"" kvg:type=""㇐"" d=""M11.62,44.6c2.63,0.78,5.51,0.5,7.28,0.17c9.98-1.89,22.62-3.29,30.72-4.06c1.29-0.12,3.13-0.21,4.36,0.12""/>
			<path id=""kvg:079c1-s3"" kvg:type=""㇑"" d=""M36.08,26.66c0.95,0.95,1.41,2.84,1.41,4.69c0,3.83,0,40.18-0.04,55.4c-0.01,3.25-0.02,5.59-0.03,6.5""/>
			<path id=""kvg:079c1-s4"" kvg:type=""㇒"" d=""M35.93,43.47c0,1.03-0.71,2.97-1.36,4.2C29.27,57.83,21.15,69.5,10.5,77.5""/>
			<path id=""kvg:079c1-s5"" kvg:type=""㇔/㇏"" d=""M41,49.75c4.4,2.51,8.13,7.52,10.5,10.75""/>
		</g>
	</g>
	<g id=""kvg:079c1-g4"" kvg:element=""厶"" kvg:position=""right"" kvg:phon=""厶"">
		<path id=""kvg:079c1-s6"" kvg:type=""㇜"" d=""M73.3,31.9c0.32,1.22,0.34,2.31-0.06,3.51C68.25,50.5,59.75,65.25,52.08,78.31c-2.12,3.61-1.6,5.03,1.88,3.89C66,78.25,76.62,74.12,88.28,69.98""/>
		<path id=""kvg:079c1-s7"" kvg:type=""㇔"" d=""M82.81,60.54c4.45,4.37,11.49,17.97,12.61,24.77""/>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_079c1"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 37.50 13.63)"">1</text>
	<text transform=""matrix(1 0 0 1 4.50 45.50)"">2</text>
	<text transform=""matrix(1 0 0 1 28.50 34.63)"">3</text>
	<text transform=""matrix(1 0 0 1 23.50 54.50)"">4</text>
	<text transform=""matrix(1 0 0 1 47.25 50.50)"">5</text>
	<text transform=""matrix(1 0 0 1 64.50 31.50)"">6</text>
	<text transform=""matrix(1 0 0 1 76.50 61.50)"">7</text>
</g>
</svg>
",
                OrderIndex = 1,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("f8dcba6a-1d53-5a46-a209-39b9596b2a11"),
                LessonId = lesson1Id,
                Level = "N5",
                Character = "日",
                HanViet = "NHẬT",
                Meaning = "Mặt trời, ngày",
                StrokeCount = 4,
                KunReading = "ひ、か",
                OnReading = "ニチ、ジツ",
                Mnemonic = "Hình vuông có tia sáng bên trong tượng trưng cho mặt trời (日).",
                ComponentMapJson = @"[]",
                StrokeDataJson = @"[""M31.5,24.5c1.12,1.12,1.74,2.75,1.74,4.75c0,1.6-0.16,38.11-0.09,53.5c0.02,3.82,0.05,6.35,0.09,6.75"", ""M33.48,26c0.8-0.05,37.67-3.01,40.77-3.25c3.19-0.25,5,1.75,5,4.25c0,4-0.22,40.84-0.23,56c0,3.48,0,5.72,0,6"", ""M34.22,55.25c7.78-0.5,35.9-2.5,44.06-2.75"", ""M34.23,86.5c10.52-0.75,34.15-2.12,43.81-2.25""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_065e5"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:065e5"" kvg:element=""日"" kvg:radical=""general"">
	<path id=""kvg:065e5-s1"" kvg:type=""㇑"" d=""M31.5,24.5c1.12,1.12,1.74,2.75,1.74,4.75c0,1.6-0.16,38.11-0.09,53.5c0.02,3.82,0.05,6.35,0.09,6.75""/>
	<path id=""kvg:065e5-s2"" kvg:type=""㇕a"" d=""M33.48,26c0.8-0.05,37.67-3.01,40.77-3.25c3.19-0.25,5,1.75,5,4.25c0,4-0.22,40.84-0.23,56c0,3.48,0,5.72,0,6""/>
	<path id=""kvg:065e5-s3"" kvg:type=""㇐a"" d=""M34.22,55.25c7.78-0.5,35.9-2.5,44.06-2.75""/>
	<path id=""kvg:065e5-s4"" kvg:type=""㇐a"" d=""M34.23,86.5c10.52-0.75,34.15-2.12,43.81-2.25""/>
</g>
</g>
<g id=""kvg:StrokeNumbers_065e5"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 25.25 32.63)"">1</text>
	<text transform=""matrix(1 0 0 1 34.50 22.50)"">2</text>
	<text transform=""matrix(1 0 0 1 37.50 51.50)"">3</text>
	<text transform=""matrix(1 0 0 1 37.50 83.50)"">4</text>
</g>
</svg>
",
                OrderIndex = 2,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("5653afa6-d7a0-5c1f-b172-e8808f8fb825"),
                LessonId = lesson1Id,
                Level = "N5",
                Character = "本",
                HanViet = "BẢN",
                Meaning = "Gốc, sách, căn bản",
                StrokeCount = 5,
                KunReading = "もと",
                OnReading = "ホン",
                Mnemonic = "Nét ngang (一) chỉ vào gốc của cây (木) tạo thành chữ gốc, căn bản (本).",
                ComponentMapJson = @"[{""character"": ""木"", ""component"": ""木"", ""name"": ""cây"", ""meaning"": ""cây""}, {""character"": ""一"", ""component"": ""一"", ""name"": ""gốc cây"", ""meaning"": ""gốc cây""}]",
                StrokeDataJson = @"[""M20.5,33.5c1.93,0.62,4.91,1.07,8.1,0.75C42.43,32.88,66,30.75,79.64,30c3.2-0.18,7.22,0.25,9.23,0.5"", ""M52.1,11.12c1.25,1.25,2.05,3.23,2.05,4.99c0,0.84,0,57.16-0.02,76.76c-0.01,3.96-0.01,6.42-0.02,6.62"", ""M51.75,33.5c0,1-0.41,2.22-1.29,3.88C43.62,50.25,30.12,65.5,13.25,75.5"", ""M54.75,35.5c4.92,5.74,23.48,23.33,32.85,31.27c2.58,2.18,5.16,4.41,8.52,5.23"", ""M33.88,73.92c1.5,0.46,2.74,0.75,5.3,0.59c9.95-0.63,21.2-2.13,27.96-2.95c1.93-0.23,3.62-0.31,6-0.02""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_0672c"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:0672c"" kvg:element=""本"">
	<g id=""kvg:0672c-g1"" kvg:element=""木"" kvg:radical=""tradit"">
		<path id=""kvg:0672c-s1"" kvg:type=""㇐"" d=""M20.5,33.5c1.93,0.62,4.91,1.07,8.1,0.75C42.43,32.88,66,30.75,79.64,30c3.2-0.18,7.22,0.25,9.23,0.5""/>
		<g id=""kvg:0672c-g2"" kvg:element=""丨"" kvg:radical=""nelson"">
			<path id=""kvg:0672c-s2"" kvg:type=""㇑"" d=""M52.1,11.12c1.25,1.25,2.05,3.23,2.05,4.99c0,0.84,0,57.16-0.02,76.76c-0.01,3.96-0.01,6.42-0.02,6.62""/>
		</g>
		<path id=""kvg:0672c-s3"" kvg:type=""㇒"" d=""M51.75,33.5c0,1-0.41,2.22-1.29,3.88C43.62,50.25,30.12,65.5,13.25,75.5""/>
		<path id=""kvg:0672c-s4"" kvg:type=""㇏"" d=""M54.75,35.5c4.92,5.74,23.48,23.33,32.85,31.27c2.58,2.18,5.16,4.41,8.52,5.23""/>
	</g>
	<path id=""kvg:0672c-s5"" kvg:type=""㇐"" d=""M33.88,73.92c1.5,0.46,2.74,0.75,5.3,0.59c9.95-0.63,21.2-2.13,27.96-2.95c1.93-0.23,3.62-0.31,6-0.02""/>
</g>
</g>
<g id=""kvg:StrokeNumbers_0672c"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 13.50 34.50)"">1</text>
	<text transform=""matrix(1 0 0 1 42.50 11.50)"">2</text>
	<text transform=""matrix(1 0 0 1 38.50 44.50)"">3</text>
	<text transform=""matrix(1 0 0 1 66.50 43.50)"">4</text>
	<text transform=""matrix(1 0 0 1 26.50 77.50)"">5</text>
</g>
</svg>
",
                OrderIndex = 3,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("beac45f0-e750-5dfd-8226-a870eb4cbd24"),
                LessonId = lesson1Id,
                Level = "N5",
                Character = "大",
                HanViet = "ĐẠI",
                Meaning = "To, lớn",
                StrokeCount = 3,
                KunReading = "おおきい",
                OnReading = "ダイ、タイ",
                Mnemonic = "Người dang rộng tay chân thể hiện sự to lớn (大).",
                ComponentMapJson = @"[{""character"": ""一"", ""component"": ""一"", ""name"": ""一"", ""meaning"": ""一""}, {""character"": ""人"", ""component"": ""人"", ""name"": ""人"", ""meaning"": ""人""}]",
                StrokeDataJson = @"[""M19.38,48.25c1.49,0.51,5.03,0.89,7.6,0.49C41.12,46.5,63,43,77.19,42.44c2.7-0.11,4.87-0.06,7.31,0.33"", ""M49.5,18c0.88,2.12,1.03,4.16,0.99,6.32C50,57,37.75,81.12,18,91.75"", ""M49.5,46c9,10.5,28.5,36.25,37.49,43.28c3.06,2.39,5.62,3.75,7.01,3.97""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_05927"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:05927"" kvg:element=""大"" kvg:radical=""general"">
	<path id=""kvg:05927-s1"" kvg:type=""㇐"" d=""M19.38,48.25c1.49,0.51,5.03,0.89,7.6,0.49C41.12,46.5,63,43,77.19,42.44c2.7-0.11,4.87-0.06,7.31,0.33""/>
	<path id=""kvg:05927-s2"" kvg:type=""㇒"" d=""M49.5,18c0.88,2.12,1.03,4.16,0.99,6.32C50,57,37.75,81.12,18,91.75""/>
	<path id=""kvg:05927-s3"" kvg:type=""㇏"" d=""M49.5,46c9,10.5,28.5,36.25,37.49,43.28c3.06,2.39,5.62,3.75,7.01,3.97""/>
</g>
</g>
<g id=""kvg:StrokeNumbers_05927"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 12.75 49.63)"">1</text>
	<text transform=""matrix(1 0 0 1 40.50 18.50)"">2</text>
	<text transform=""matrix(1 0 0 1 61.50 55.63)"">3</text>
</g>
</svg>
",
                OrderIndex = 4,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("8e5758ae-b26b-57b7-a0d9-bdd56ffc6104"),
                LessonId = lesson1Id,
                Level = "N5",
                Character = "学",
                HanViet = "HỌC",
                Meaning = "Học tập",
                StrokeCount = 8,
                KunReading = "まなぶ",
                OnReading = "ガク",
                Mnemonic = "Đứa trẻ (子) ngồi học dưới mái che (冖) tạo thành học tập (学).",
                ComponentMapJson = @"[{""character"": ""⺍"", ""component"": ""⺍"", ""name"": ""mái nhỏ / ánh sáng"", ""meaning"": ""mái nhỏ / ánh sáng""}, {""character"": ""冖"", ""component"": ""冖"", ""name"": ""mái che"", ""meaning"": ""mái che""}, {""character"": ""子"", ""component"": ""子"", ""name"": ""trẻ em"", ""meaning"": ""trẻ em""}]",
                StrokeDataJson = @"[""M29.5,17.25c3.5,3,6.5,7.25,7.75,9.75"", ""M49,12c1.25,2,4.75,8.25,5.25,11.5"", ""M75,11c0.25,1.75-0.12,2.75-0.75,4.25c-1.29,3.1-4.25,7.38-6.5,9.75"", ""M21.25,33.75c-0.12,4.75-2,12.5-3.75,16.25"", ""M23.5,36.5c17-1.62,42.38-5.5,60-5.75c9.5-0.13,4.12,5.12,0,9"", ""M37.25,46.5c1,0.25,3.75,0.25,5.5-0.25s18.25-4,20-4s2.75,0.75,1,2.25S54.5,53.5,53,54.75"", ""M50.75,55.75c4,8.75,7.18,24.67,1.75,38c-2.75,6.75-7.75,1.25-9.75-2"", ""M15.75,67.75c1.75,1,4.64,1.36,7.5,1c15.88-2,44.43-6.25,61.37-5.5c2.5,0.11,4.72,0.25,6.39,1""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_05b66"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:05b66"" kvg:element=""学"">
	<g id=""kvg:05b66-g1"" kvg:position=""top"" kvg:phon=""𦥯"">
		<g id=""kvg:05b66-g2"" kvg:element=""⺍"" kvg:original=""つ"">
			<path id=""kvg:05b66-s1"" kvg:type=""㇔"" d=""M29.5,17.25c3.5,3,6.5,7.25,7.75,9.75""/>
			<path id=""kvg:05b66-s2"" kvg:type=""㇔"" d=""M49,12c1.25,2,4.75,8.25,5.25,11.5""/>
			<path id=""kvg:05b66-s3"" kvg:type=""㇒"" d=""M75,11c0.25,1.75-0.12,2.75-0.75,4.25c-1.29,3.1-4.25,7.38-6.5,9.75""/>
		</g>
		<g id=""kvg:05b66-g3"" kvg:element=""冖"">
			<path id=""kvg:05b66-s4"" kvg:type=""㇔"" d=""M21.25,33.75c-0.12,4.75-2,12.5-3.75,16.25""/>
			<path id=""kvg:05b66-s5"" kvg:type=""㇖b"" d=""M23.5,36.5c17-1.62,42.38-5.5,60-5.75c9.5-0.13,4.12,5.12,0,9""/>
		</g>
	</g>
	<g id=""kvg:05b66-g4"" kvg:element=""子"" kvg:position=""bottom"" kvg:radical=""general"">
		<path id=""kvg:05b66-s6"" kvg:type=""㇖"" d=""M37.25,46.5c1,0.25,3.75,0.25,5.5-0.25s18.25-4,20-4s2.75,0.75,1,2.25S54.5,53.5,53,54.75""/>
		<path id=""kvg:05b66-s7"" kvg:type=""㇁"" d=""M50.75,55.75c4,8.75,7.18,24.67,1.75,38c-2.75,6.75-7.75,1.25-9.75-2""/>
		<path id=""kvg:05b66-s8"" kvg:type=""㇐"" d=""M15.75,67.75c1.75,1,4.64,1.36,7.5,1c15.88-2,44.43-6.25,61.37-5.5c2.5,0.11,4.72,0.25,6.39,1""/>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_05b66"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 20.25 13.63)"">1</text>
	<text transform=""matrix(1 0 0 1 39.75 8.50)"">2</text>
	<text transform=""matrix(1 0 0 1 66.50 8.50)"">3</text>
	<text transform=""matrix(1 0 0 1 14.50 34.63)"">4</text>
	<text transform=""matrix(1 0 0 1 24.50 32.50)"">5</text>
	<text transform=""matrix(1 0 0 1 29.25 48.50)"">6</text>
	<text transform=""matrix(1 0 0 1 44.50 58.63)"">7</text>
	<text transform=""matrix(1 0 0 1 8.50 72.50)"">8</text>
</g>
</svg>
",
                OrderIndex = 5,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("64d836b9-4fd7-50ac-81f8-6da9defab802"),
                LessonId = lesson1Id,
                Level = "N5",
                Character = "語",
                HanViet = "NGỮ",
                Meaning = "Ngôn ngữ, tiếng nói",
                StrokeCount = 14,
                KunReading = "かたる",
                OnReading = "ゴ",
                Mnemonic = "Dùng lời nói (言) của bản thân (吾) để tạo thành ngôn ngữ (語).",
                ComponentMapJson = @"[{""character"": ""言"", ""component"": ""言"", ""name"": ""lời nói"", ""meaning"": ""lời nói""}, {""character"": ""吾"", ""component"": ""吾"", ""name"": ""ta / bản thân"", ""meaning"": ""ta / bản thân""}]",
                StrokeDataJson = @"[""M26,15.25c2.82,1.41,7.29,5.8,8,8"", ""M12.37,32.97c1.25,0.28,2.88,0.66,4.36,0.53c7.02-0.59,17.78-1.75,25.95-3c1.52-0.23,3.57-0.38,5.16,0.03"", ""M18.73,45.76c0.38,0.18,2.71,0.2,3.1,0.18c3.97-0.21,9.79-1.19,14.46-2.31c1.67-0.4,2.71-0.38,3.86-0.08"", ""M18.73,58.89c0.89,0.23,1.89,0.36,3.35,0.15c3.89-0.54,10.71-1.51,14.85-2.29c0.7-0.13,1.82-0.26,2.61-0.1"", ""M17.14,71.9c0.63,0.62,1.12,1.65,1.23,2.57c0.63,5.03,1.51,10.28,2.23,15.59c0.14,1.03,0.27,2.02,0.41,2.93"", ""M19.37,73.6c5.67-0.94,15.47-2.73,20.36-3.48c1.49-0.22,2.39,1.05,2.18,2.08c-0.71,3.44-2.27,9.75-3.23,13.89"", ""M21.47,89.02c3.95-0.45,10.71-1.19,16.28-1.61c1.21-0.09,2.36-0.17,3.41-0.22"", ""M51.79,17.49c1.38,0.26,3.91,0.28,5.27,0.15C63.88,17,72.62,15.62,80,15.32c2.3-0.1,3.67,0.04,4.81,0.15"", ""M67.75,20.25c0.37,1.25,0.5,2.38,0.23,3.75c-0.75,3.78-6.03,23.83-7.96,31.58"", ""M52.18,36.96c1.82,0.66,4.17,0.95,5.84,0.66c8.48-1.5,16.13-3.06,22.74-4.1c2.49-0.39,4.05,1.27,3.71,2.93c-0.6,2.93-2.48,11.43-3.74,17.86"", ""M46.33,58.46c1.13,0.24,3.94,0.2,5.07,0.08c12.34-1.29,19.11-2.39,40.88-4.02c1.88-0.14,3.75-0.02,4.69,0.09"", ""M52.5,69.88c0.93,0.93,1.42,2.28,1.54,3.31c0.71,6.06,1.42,12.65,2.06,19.3c0.15,1.5,0.28,2.44,0.4,3.75"", ""M54.99,71.67c9.47-1.45,23.75-3.41,28.85-3.9c2.14-0.21,3.28,0.98,2.86,2.93c-0.84,3.88-3.08,12.57-4.39,17.58"", ""M57.2,91.49c5.94-0.55,14.67-1.24,23.54-1.76c1.3-0.08,2.63-0.13,3.97-0.2""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_08a9e"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:08a9e"" kvg:element=""語"">
	<g id=""kvg:08a9e-g1"" kvg:element=""言"" kvg:position=""left"" kvg:radical=""general"">
		<path id=""kvg:08a9e-s1"" kvg:type=""㇔"" d=""M26,15.25c2.82,1.41,7.29,5.8,8,8""/>
		<path id=""kvg:08a9e-s2"" kvg:type=""㇐"" d=""M12.37,32.97c1.25,0.28,2.88,0.66,4.36,0.53c7.02-0.59,17.78-1.75,25.95-3c1.52-0.23,3.57-0.38,5.16,0.03""/>
		<path id=""kvg:08a9e-s3"" kvg:type=""㇐"" d=""M18.73,45.76c0.38,0.18,2.71,0.2,3.1,0.18c3.97-0.21,9.79-1.19,14.46-2.31c1.67-0.4,2.71-0.38,3.86-0.08""/>
		<path id=""kvg:08a9e-s4"" kvg:type=""㇐"" d=""M18.73,58.89c0.89,0.23,1.89,0.36,3.35,0.15c3.89-0.54,10.71-1.51,14.85-2.29c0.7-0.13,1.82-0.26,2.61-0.1""/>
		<g id=""kvg:08a9e-g2"" kvg:element=""口"">
			<path id=""kvg:08a9e-s5"" kvg:type=""㇑"" d=""M17.14,71.9c0.63,0.62,1.12,1.65,1.23,2.57c0.63,5.03,1.51,10.28,2.23,15.59c0.14,1.03,0.27,2.02,0.41,2.93""/>
			<path id=""kvg:08a9e-s6"" kvg:type=""㇕b"" d=""M19.37,73.6c5.67-0.94,15.47-2.73,20.36-3.48c1.49-0.22,2.39,1.05,2.18,2.08c-0.71,3.44-2.27,9.75-3.23,13.89""/>
			<path id=""kvg:08a9e-s7"" kvg:type=""㇐b"" d=""M21.47,89.02c3.95-0.45,10.71-1.19,16.28-1.61c1.21-0.09,2.36-0.17,3.41-0.22""/>
		</g>
	</g>
	<g id=""kvg:08a9e-g3"" kvg:element=""吾"" kvg:position=""right"" kvg:phon=""吾"">
		<g id=""kvg:08a9e-g4"" kvg:element=""五"">
			<g id=""kvg:08a9e-g5"" kvg:element=""二"" kvg:part=""1"">
				<path id=""kvg:08a9e-s8"" kvg:type=""㇐"" d=""M51.79,17.49c1.38,0.26,3.91,0.28,5.27,0.15C63.88,17,72.62,15.62,80,15.32c2.3-0.1,3.67,0.04,4.81,0.15""/>
			</g>
			<path id=""kvg:08a9e-s9"" kvg:type=""㇑a"" d=""M67.75,20.25c0.37,1.25,0.5,2.38,0.23,3.75c-0.75,3.78-6.03,23.83-7.96,31.58""/>
			<path id=""kvg:08a9e-s10"" kvg:type=""㇕c"" d=""M52.18,36.96c1.82,0.66,4.17,0.95,5.84,0.66c8.48-1.5,16.13-3.06,22.74-4.1c2.49-0.39,4.05,1.27,3.71,2.93c-0.6,2.93-2.48,11.43-3.74,17.86""/>
			<g id=""kvg:08a9e-g6"" kvg:element=""二"" kvg:part=""2"">
				<path id=""kvg:08a9e-s11"" kvg:type=""㇐"" d=""M46.33,58.46c1.13,0.24,3.94,0.2,5.07,0.08c12.34-1.29,19.11-2.39,40.88-4.02c1.88-0.14,3.75-0.02,4.69,0.09""/>
			</g>
		</g>
		<g id=""kvg:08a9e-g7"" kvg:element=""口"">
			<path id=""kvg:08a9e-s12"" kvg:type=""㇑"" d=""M52.5,69.88c0.93,0.93,1.42,2.28,1.54,3.31c0.71,6.06,1.42,12.65,2.06,19.3c0.15,1.5,0.28,2.44,0.4,3.75""/>
			<path id=""kvg:08a9e-s13"" kvg:type=""㇕b"" d=""M54.99,71.67c9.47-1.45,23.75-3.41,28.85-3.9c2.14-0.21,3.28,0.98,2.86,2.93c-0.84,3.88-3.08,12.57-4.39,17.58""/>
			<path id=""kvg:08a9e-s14"" kvg:type=""㇐b"" d=""M57.2,91.49c5.94-0.55,14.67-1.24,23.54-1.76c1.3-0.08,2.63-0.13,3.97-0.2""/>
		</g>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_08a9e"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 20.25 13.63)"">1</text>
	<text transform=""matrix(1 0 0 1 3.75 33.13)"">2</text>
	<text transform=""matrix(1 0 0 1 9.75 45.13)"">3</text>
	<text transform=""matrix(1 0 0 1 9.75 58.63)"">4</text>
	<text transform=""matrix(1 0 0 1 9.75 73.63)"">5</text>
	<text transform=""matrix(1 0 0 1 20.25 70.63)"">6</text>
	<text transform=""matrix(1 0 0 1 24.75 85.63)"">7</text>
	<text transform=""matrix(1 0 0 1 50.25 13.63)"">8</text>
	<text transform=""matrix(1 0 0 1 74.25 25.63)"">9</text>
	<text transform=""matrix(1 0 0 1 51.75 34.63)"">10</text>
	<text transform=""matrix(1 0 0 1 47.25 54.13)"">11</text>
	<text transform=""matrix(1 0 0 1 44.25 85.63)"">12</text>
	<text transform=""matrix(1 0 0 1 56.25 67.63)"">13</text>
	<text transform=""matrix(1 0 0 1 60.75 87.13)"">14</text>
</g>
</svg>
",
                OrderIndex = 6,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("e095aca4-eeea-553a-bf95-716527c4e6b0"),
                LessonId = lesson1Id,
                Level = "N5",
                Character = "校",
                HanViet = "HIỆU",
                Meaning = "Trường học",
                StrokeCount = 10,
                KunReading = "",
                OnReading = "コウ",
                Mnemonic = "Nơi mọi người gặp gỡ (交) dưới sân trường có cây (木) tạo thành trường học (校).",
                ComponentMapJson = @"[{""character"": ""木"", ""component"": ""木"", ""name"": ""cây"", ""meaning"": ""cây""}, {""character"": ""交"", ""component"": ""交"", ""name"": ""giao nhau"", ""meaning"": ""giao nhau""}]",
                StrokeDataJson = @"[""M11.53,40.68c1.1,0.32,2.6,0.45,4.53,0.32c5.4-0.35,16.57-3,23.14-4.04c1.25-0.2,2.3-0.18,3.07,0"", ""M28.99,17.25c1.07,1.07,1.76,3.25,1.76,5.25c0,0.77-0.03,48.09-0.18,65.25c-0.03,3.03-0.05,5.16-0.07,6"", ""M30.25,40.75c0,1.25-0.49,2.66-0.96,3.77C25.28,53.91,20.88,62.25,15,70"", ""M33.75,51.25c2.75,1.5,6,5.25,7.25,7.75"", ""M66.39,15.5c0.99,0.99,1.38,1.88,1.38,3.62c0,4.25-0.02,7.62-0.08,10.41"", ""M48.12,31.71c2.3,0.29,3.9,0.44,6.09,0.2c10.28-1.16,20.32-2.66,32.45-3.53c2.35-0.17,4.03-0.01,5.33,0.32"", ""M59.24,38.93c0.2,0.53,0.06,2.27-0.4,3.14C57,45.5,53.75,49.5,50,52.5"", ""M79.27,38.5c4.34,3.07,8.73,8.68,10.9,12.41"", ""M79.15,49.18c0.35,1.32,0.17,2.62-0.54,4.18C72.25,67.25,58.75,83.25,44,91.25"", ""M55.95,55.88c6.3,3.37,21.64,22.12,31.45,30.33c2.64,2.21,5.07,4.15,8.6,4.44""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_06821"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:06821"" kvg:element=""校"">
	<g id=""kvg:06821-g1"" kvg:element=""木"" kvg:position=""left"" kvg:radical=""general"">
		<path id=""kvg:06821-s1"" kvg:type=""㇐"" d=""M11.53,40.68c1.1,0.32,2.6,0.45,4.53,0.32c5.4-0.35,16.57-3,23.14-4.04c1.25-0.2,2.3-0.18,3.07,0""/>
		<path id=""kvg:06821-s2"" kvg:type=""㇑"" d=""M28.99,17.25c1.07,1.07,1.76,3.25,1.76,5.25c0,0.77-0.03,48.09-0.18,65.25c-0.03,3.03-0.05,5.16-0.07,6""/>
		<path id=""kvg:06821-s3"" kvg:type=""㇒"" d=""M30.25,40.75c0,1.25-0.49,2.66-0.96,3.77C25.28,53.91,20.88,62.25,15,70""/>
		<path id=""kvg:06821-s4"" kvg:type=""㇔/㇏"" d=""M33.75,51.25c2.75,1.5,6,5.25,7.25,7.75""/>
	</g>
	<g id=""kvg:06821-g2"" kvg:element=""交"" kvg:position=""right"" kvg:phon=""交"">
		<g id=""kvg:06821-g3"" kvg:element=""亠"">
			<path id=""kvg:06821-s5"" kvg:type=""㇑a"" d=""M66.39,15.5c0.99,0.99,1.38,1.88,1.38,3.62c0,4.25-0.02,7.62-0.08,10.41""/>
			<path id=""kvg:06821-s6"" kvg:type=""㇐"" d=""M48.12,31.71c2.3,0.29,3.9,0.44,6.09,0.2c10.28-1.16,20.32-2.66,32.45-3.53c2.35-0.17,4.03-0.01,5.33,0.32""/>
		</g>
		<g id=""kvg:06821-g4"" kvg:element=""父"">
			<path id=""kvg:06821-s7"" kvg:type=""㇒"" d=""M59.24,38.93c0.2,0.53,0.06,2.27-0.4,3.14C57,45.5,53.75,49.5,50,52.5""/>
			<path id=""kvg:06821-s8"" kvg:type=""㇔"" d=""M79.27,38.5c4.34,3.07,8.73,8.68,10.9,12.41""/>
			<path id=""kvg:06821-s9"" kvg:type=""㇒"" d=""M79.15,49.18c0.35,1.32,0.17,2.62-0.54,4.18C72.25,67.25,58.75,83.25,44,91.25""/>
			<path id=""kvg:06821-s10"" kvg:type=""㇏"" d=""M55.95,55.88c6.3,3.37,21.64,22.12,31.45,30.33c2.64,2.21,5.07,4.15,8.6,4.44""/>
		</g>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_06821"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 5.50 41.50)"">1</text>
	<text transform=""matrix(1 0 0 1 18.75 16.50)"">2</text>
	<text transform=""matrix(1 0 0 1 19.50 52.50)"">3</text>
	<text transform=""matrix(1 0 0 1 35.50 49.50)"">4</text>
	<text transform=""matrix(1 0 0 1 58.50 12.50)"">5</text>
	<text transform=""matrix(1 0 0 1 47.25 28.50)"">6</text>
	<text transform=""matrix(1 0 0 1 50.50 41.50)"">7</text>
	<text transform=""matrix(1 0 0 1 70.50 40.50)"">8</text>
	<text transform=""matrix(1 0 0 1 71.50 50.50)"">9</text>
	<text transform=""matrix(1 0 0 1 61.50 58.50)"">10</text>
</g>
</svg>
",
                OrderIndex = 7,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("aabacd7a-7cf8-5318-a09f-7db6bd98ca00"),
                LessonId = lesson1Id,
                Level = "N5",
                Character = "生",
                HanViet = "SINH",
                Meaning = "Sinh sống, sinh ra",
                StrokeCount = 5,
                KunReading = "いきる、うまれる、なま",
                OnReading = "セイ、ショウ",
                Mnemonic = "Mầm cây mọc lên từ đất tạo thành sự sống (生).",
                ComponentMapJson = @"[{""character"": ""土"", ""component"": ""土"", ""name"": ""đất"", ""meaning"": ""đất""}]",
                StrokeDataJson = @"[""M31.26,25.89c0.36,1.36,0.35,2.65-0.05,3.79c-2.34,6.69-7.24,17.22-14.96,24.19"", ""M31.13,40.67c2.37,0.33,4.03,0.07,5.64-0.12c9.5-1.1,25.15-4.12,35.35-5.83c2.51-0.42,4.86-0.73,7.38-0.33"", ""M52.31,12.63c1.28,1.28,2.01,3.12,2.01,5.23c0,4.01,0,65.14,0,69.77"", ""M29.38,64.03c2.64,0.67,5.38,0.31,8.04-0.02C49.45,62.51,62.16,61,72.5,59.86c2.38-0.26,4.99-0.76,7.38-0.23"", ""M15.75,90.25c3.04,0.75,6.21,0.94,8.4,0.8C40.62,90,68.12,86.5,83.3,85.75c3.63-0.18,7.68,0,10.07,0.73""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_0751f"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:0751f"" kvg:element=""生"" kvg:radical=""general"">
	<path id=""kvg:0751f-s1"" kvg:type=""㇒"" d=""M31.26,25.89c0.36,1.36,0.35,2.65-0.05,3.79c-2.34,6.69-7.24,17.22-14.96,24.19""/>
	<path id=""kvg:0751f-s2"" kvg:type=""㇐"" d=""M31.13,40.67c2.37,0.33,4.03,0.07,5.64-0.12c9.5-1.1,25.15-4.12,35.35-5.83c2.51-0.42,4.86-0.73,7.38-0.33""/>
	<path id=""kvg:0751f-s3"" kvg:type=""㇑a"" d=""M52.31,12.63c1.28,1.28,2.01,3.12,2.01,5.23c0,4.01,0,65.14,0,69.77""/>
	<path id=""kvg:0751f-s4"" kvg:type=""㇐"" d=""M29.38,64.03c2.64,0.67,5.38,0.31,8.04-0.02C49.45,62.51,62.16,61,72.5,59.86c2.38-0.26,4.99-0.76,7.38-0.23""/>
	<path id=""kvg:0751f-s5"" kvg:type=""㇐"" d=""M15.75,90.25c3.04,0.75,6.21,0.94,8.4,0.8C40.62,90,68.12,86.5,83.3,85.75c3.63-0.18,7.68,0,10.07,0.73""/>
</g>
</g>
<g id=""kvg:StrokeNumbers_0751f"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 22.75 25.63)"">1</text>
	<text transform=""matrix(1 0 0 1 36.00 37.28)"">2</text>
	<text transform=""matrix(1 0 0 1 42.50 10.50)"">3</text>
	<text transform=""matrix(1 0 0 1 21.50 66.13)"">4</text>
	<text transform=""matrix(1 0 0 1 8.50 89.50)"">5</text>
</g>
</svg>
",
                OrderIndex = 8,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("22d23b80-2c89-5e0b-8b0d-0789864c4909"),
                LessonId = lesson1Id,
                Level = "N5",
                Character = "人",
                HanViet = "NHÂN",
                Meaning = "Người",
                StrokeCount = 2,
                KunReading = "ひと",
                OnReading = "ジン、ニン",
                Mnemonic = "Hai nét giống dáng người đang bước đi tạo thành chữ người (人).",
                ComponentMapJson = @"[]",
                StrokeDataJson = @"[""M54.5,20c0.37,2.12,0.23,4.03-0.22,6.27C51.68,39.48,38.25,72.25,16.5,87.25"", ""M46,54.25c6.12,6,25.51,22.24,35.52,29.72c3.66,2.73,6.94,4.64,11.48,5.53""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_04eba"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:04eba"" kvg:element=""人"" kvg:radical=""general"">
	<path id=""kvg:04eba-s1"" kvg:type=""㇒"" d=""M54.5,20c0.37,2.12,0.23,4.03-0.22,6.27C51.68,39.48,38.25,72.25,16.5,87.25""/>
	<path id=""kvg:04eba-s2"" kvg:type=""㇏"" d=""M46,54.25c6.12,6,25.51,22.24,35.52,29.72c3.66,2.73,6.94,4.64,11.48,5.53""/>
</g>
</g>
<g id=""kvg:StrokeNumbers_04eba"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 45.50 19.50)"">1</text>
	<text transform=""matrix(1 0 0 1 52.50 55.63)"">2</text>
</g>
</svg>
",
                OrderIndex = 9,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("b72ae3b6-8a3a-58a9-a43e-e73f8d0f3c3d"),
                LessonId = lesson1Id,
                Level = "N5",
                Character = "才",
                HanViet = "TÀI",
                Meaning = "Tuổi, tài năng",
                StrokeCount = 3,
                KunReading = "",
                OnReading = "サイ",
                Mnemonic = "Mỗi năm thêm một dấu mốc trưởng thành tạo thành tuổi tác, tài năng (才).",
                ComponentMapJson = @"[{""character"": ""十"", ""component"": ""十"", ""name"": ""mười"", ""meaning"": ""mười""}]",
                StrokeDataJson = @"[""M 18.62,38.5 c 2.62,0.62 4.54,0.53 7.82,0.25 C 41.25,37.5 67.62,35 84.75,34 c 3.28,-0.19 5.86,0 8,0.5"", ""M 60.77,13.58 c 1.39,1.39 2.26,2.99 2.26,6.02 0,15.4 -0.01,66.41 -0.01,71.37 0,8.78 -7.21,0.5 -8.71,-0.75"", ""M 73.043742,46.367587 C 62.856133,57.87424 32.711367,80.494361 23.641472,85.467096""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_0624d"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:0624d"" kvg:element=""才"">
	<g id=""kvg:0624d-g1"" kvg:element=""扌"" kvg:variant=""true"" kvg:original=""手"" kvg:radical=""tradit"">
		<path id=""kvg:0624d-s1"" kvg:type=""㇐"" d=""M 18.62,38.5 c 2.62,0.62 4.54,0.53 7.82,0.25 C 41.25,37.5 67.62,35 84.75,34 c 3.28,-0.19 5.86,0 8,0.5""/>
		<g id=""kvg:0624d-g2"" kvg:element=""亅"" kvg:radical=""nelson"">
			<path id=""kvg:0624d-s2"" kvg:type=""㇚"" d=""M 60.77,13.58 c 1.39,1.39 2.26,2.99 2.26,6.02 0,15.4 -0.01,66.41 -0.01,71.37 0,8.78 -7.21,0.5 -8.71,-0.75""/>
		</g>
		<path id=""kvg:0624d-s3"" kvg:type=""㇒"" d=""M 73.043742,46.367587 C 62.856133,57.87424 32.711367,80.494361 23.641472,85.467096""/>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_0624d"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 11.50 38.50)"">1</text>
	<text transform=""matrix(1 0 0 1 53.73 16.07)"">2</text>
	<text transform=""matrix(1 0 0 1 75.49 46.06)"">3</text>
</g>
</svg>
",
                OrderIndex = 10,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
        };

        foreach (var item in kanjiItems1)
        {
            if (!await db.KanjiItems.AnyAsync(k => k.Character == item.Character))
                db.KanjiItems.Add(item);
        }

        var vocabItems1 = new List<KanjiVocabulary>
        {
            new KanjiVocabulary {
                Id = Guid.Parse("cdaeb163-9a25-5b9e-86f2-ddbda4570e56"),
                LessonId = lesson1Id,
                KanjiItemId = Guid.Parse("28bf8b0c-7514-5252-b44f-4cae34d664a9"),
                Level = "N5",
                Word = "私",
                Reading = "わたし",
                Meaning = "tôi",
                OrderIndex = 1,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("c35c5d80-1543-5d1e-9282-bc7ff0188c52"),
                LessonId = lesson1Id,
                KanjiItemId = Guid.Parse("22d23b80-2c89-5e0b-8b0d-0789864c4909"),
                Level = "N5",
                Word = "人",
                Reading = "ひと",
                Meaning = "Con người",
                OrderIndex = 2,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("00aba2de-0b52-55ce-9c4c-ef99ddd712c7"),
                LessonId = lesson1Id,
                KanjiItemId = Guid.Parse("22d23b80-2c89-5e0b-8b0d-0789864c4909"),
                Level = "N5",
                Word = "あの人",
                Reading = "あのひと",
                Meaning = "Người kia",
                OrderIndex = 3,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("7a0f53b8-22ef-5e68-906f-b98442cffc6b"),
                LessonId = lesson1Id,
                KanjiItemId = Guid.Parse("b72ae3b6-8a3a-58a9-a43e-e73f8d0f3c3d"),
                Level = "N5",
                Word = "2才",
                Reading = "にさい",
                Meaning = "2 tuổi",
                OrderIndex = 4,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("09d05fa8-94bf-5901-b162-d24c937614c7"),
                LessonId = lesson1Id,
                KanjiItemId = Guid.Parse("b72ae3b6-8a3a-58a9-a43e-e73f8d0f3c3d"),
                Level = "N5",
                Word = "8才",
                Reading = "はっさい",
                Meaning = "8 tuổi",
                OrderIndex = 5,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("909a8d9b-54bc-5309-b39b-4e83b0f6ce67"),
                LessonId = lesson1Id,
                KanjiItemId = Guid.Parse("b72ae3b6-8a3a-58a9-a43e-e73f8d0f3c3d"),
                Level = "N5",
                Word = "何才",
                Reading = "なんさい",
                Meaning = "Mấy tuổi",
                OrderIndex = 6,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("2e01b767-d065-5f41-abe3-a26b9d519e93"),
                LessonId = lesson1Id,
                KanjiItemId = Guid.Parse("beac45f0-e750-5dfd-8226-a870eb4cbd24"),
                Level = "N5",
                Word = "大学",
                Reading = "だいがく",
                Meaning = "Đại học",
                OrderIndex = 7,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("6f5eb9ba-6995-5d48-8d59-c7e8507c390b"),
                LessonId = lesson1Id,
                KanjiItemId = Guid.Parse("8e5758ae-b26b-57b7-a0d9-bdd56ffc6104"),
                Level = "N5",
                Word = "学生",
                Reading = "がくせい",
                Meaning = "Học sinh",
                OrderIndex = 8,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("786a48f5-5070-5272-bd32-69ba3eade111"),
                LessonId = lesson1Id,
                KanjiItemId = Guid.Parse("aabacd7a-7cf8-5318-a09f-7db6bd98ca00"),
                Level = "N5",
                Word = "先生",
                Reading = "せんせい",
                Meaning = "Thầy/cô giáo",
                OrderIndex = 9,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("870163db-a872-5eb9-8c5e-0d59e025c9d1"),
                LessonId = lesson1Id,
                KanjiItemId = Guid.Parse("8e5758ae-b26b-57b7-a0d9-bdd56ffc6104"),
                Level = "N5",
                Word = "学校",
                Reading = "がっこう",
                Meaning = "Trường học",
                OrderIndex = 10,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("0ac16f00-14f0-560a-8f6a-2dc2b440b9d9"),
                LessonId = lesson1Id,
                KanjiItemId = Guid.Parse("f8dcba6a-1d53-5a46-a209-39b9596b2a11"),
                Level = "N5",
                Word = "10日",
                Reading = "とおか",
                Meaning = "Mùng 10",
                OrderIndex = 11,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("f3bd669a-18ae-5b47-99de-5858f02522d3"),
                LessonId = lesson1Id,
                KanjiItemId = Guid.Parse("f8dcba6a-1d53-5a46-a209-39b9596b2a11"),
                Level = "N5",
                Word = "日曜日",
                Reading = "にちようび",
                Meaning = "Chủ nhật",
                OrderIndex = 12,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("e382eca1-bbb2-5cf7-b073-b2ac3afcc42d"),
                LessonId = lesson1Id,
                KanjiItemId = Guid.Parse("f8dcba6a-1d53-5a46-a209-39b9596b2a11"),
                Level = "N5",
                Word = "日",
                Reading = "ひ",
                Meaning = "Ngày",
                OrderIndex = 13,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("78718cc7-f237-5d4e-b592-9767dc47e421"),
                LessonId = lesson1Id,
                KanjiItemId = Guid.Parse("f8dcba6a-1d53-5a46-a209-39b9596b2a11"),
                Level = "N5",
                Word = "休日",
                Reading = "きゅうじつ",
                Meaning = "Ngày nghỉ",
                OrderIndex = 14,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("b7dacb17-7678-528f-9456-63108dbbbb12"),
                LessonId = lesson1Id,
                KanjiItemId = Guid.Parse("5653afa6-d7a0-5c1f-b172-e8808f8fb825"),
                Level = "N5",
                Word = "本",
                Reading = "ほん",
                Meaning = "sách",
                OrderIndex = 15,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("c015e707-2305-52da-a71a-cf6213e495be"),
                LessonId = lesson1Id,
                KanjiItemId = Guid.Parse("f8dcba6a-1d53-5a46-a209-39b9596b2a11"),
                Level = "N5",
                Word = "日本",
                Reading = "にほん",
                Meaning = "Nhật Bản",
                OrderIndex = 16,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("df1a9cd5-8c1e-5581-85a2-9baefd99e9c5"),
                LessonId = lesson1Id,
                KanjiItemId = Guid.Parse("f8dcba6a-1d53-5a46-a209-39b9596b2a11"),
                Level = "N5",
                Word = "日本人",
                Reading = "にほんじん",
                Meaning = "Người Nhật Bản",
                OrderIndex = 17,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("4653c7f0-4500-54b8-a727-8b03738c36a4"),
                LessonId = lesson1Id,
                KanjiItemId = Guid.Parse("f8dcba6a-1d53-5a46-a209-39b9596b2a11"),
                Level = "N5",
                Word = "日本語",
                Reading = "にほんご",
                Meaning = "Tiếng Nhật",
                OrderIndex = 18,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("92426bbc-31c9-562b-8410-9772baa1bbd6"),
                LessonId = lesson1Id,
                KanjiItemId = Guid.Parse("64d836b9-4fd7-50ac-81f8-6da9defab802"),
                Level = "N5",
                Word = "ベトナム語",
                Reading = "ベトナムご",
                Meaning = "Tiếng Việt",
                OrderIndex = 19,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("b84bd322-84dc-5a48-86a2-e2aecbaf9ecf"),
                LessonId = lesson1Id,
                KanjiItemId = Guid.Parse("22d23b80-2c89-5e0b-8b0d-0789864c4909"),
                Level = "N5",
                Word = "～人",
                Reading = "〜じん",
                Meaning = "người (nước ~)",
                OrderIndex = 20,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
        };

        foreach (var item in vocabItems1)
        {
            if (!await db.KanjiVocabularyItems.AnyAsync(v => v.Word == item.Word && v.Reading == item.Reading))
                db.KanjiVocabularyItems.Add(item);
        }

        // Lesson 2: Số đếm và Đơn vị tiền tệ (JPD113)
        var lesson2Id = Guid.Parse("5ef50301-f4f5-5cc9-a8a0-c49fe2168bd8");
        if (!await db.KanjiLessons.AnyAsync(l => l.Id == lesson2Id))
        {
            db.KanjiLessons.Add(new KanjiLesson { Id = lesson2Id, Level = "N5", LessonNumber = 2, Title = "Số đếm và Đơn vị tiền tệ", Description = "Số đếm và Đơn vị tiền tệ - JPD113 Kanji N5.", AccessTier = "free", PackageCode = "kanji_jpd113", OrderIndex = 2, CreatedAt = SeededAt, UpdatedAt = SeededAt });
        }

        var kanjiItems2 = new List<KanjiItem>
        {
            new KanjiItem {
                Id = Guid.Parse("f67923c2-4634-53f4-8606-fcc50c8de6c4"),
                LessonId = lesson2Id,
                Level = "N5",
                Character = "一",
                HanViet = "NHẤT",
                Meaning = "Số một",
                StrokeCount = 1,
                KunReading = "ひと.つ",
                OnReading = "イチ、イツ",
                Mnemonic = "Chỉ một nét ngang đơn giản, biểu thị số một (一).",
                ComponentMapJson = @"[]",
                StrokeDataJson = @"[""M11,54.25c3.19,0.62,6.25,0.75,9.73,0.5c20.64-1.5,50.39-5.12,68.58-5.24c3.6-0.02,5.77,0.24,7.57,0.49""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_04e00"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:04e00"" kvg:element=""一"" kvg:radical=""general"">
	<path id=""kvg:04e00-s1"" kvg:type=""㇐"" d=""M11,54.25c3.19,0.62,6.25,0.75,9.73,0.5c20.64-1.5,50.39-5.12,68.58-5.24c3.6-0.02,5.77,0.24,7.57,0.49""/>
</g>
</g>
<g id=""kvg:StrokeNumbers_04e00"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 4.25 54.13)"">1</text>
</g>
</svg>
",
                OrderIndex = 1,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("472283d7-6919-56ff-9fec-2d6aa36cc077"),
                LessonId = lesson2Id,
                Level = "N5",
                Character = "二",
                HanViet = "NHỊ",
                Meaning = "Số hai",
                StrokeCount = 2,
                KunReading = "ふた.つ",
                OnReading = "ニ",
                Mnemonic = "Hai nét ngang song song xếp chồng lên nhau, biểu thị số hai (二).",
                ComponentMapJson = @"[]",
                StrokeDataJson = @"[""M25.25,32.4c1.77,0.37,4.78,0.56,6.55,0.37c10.82-1.15,28.82-3.4,41.24-3.76c2.95-0.09,4.73,0.18,6.21,0.36"", ""M12,80.75c2.37,0.5,6.73,0.67,9.09,0.5c23.79-1.75,45.04-4.12,67.49-4.74c3.95-0.11,6.32,0.24,8.3,0.49""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_04e8c"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:04e8c"" kvg:element=""二"" kvg:radical=""general"">
	<g id=""kvg:04e8c-g1"" kvg:position=""top"">
		<path id=""kvg:04e8c-s1"" kvg:type=""㇐"" d=""M25.25,32.4c1.77,0.37,4.78,0.56,6.55,0.37c10.82-1.15,28.82-3.4,41.24-3.76c2.95-0.09,4.73,0.18,6.21,0.36""/>
	</g>
	<g id=""kvg:04e8c-g2"" kvg:position=""bottom"">
		<path id=""kvg:04e8c-s2"" kvg:type=""㇐"" d=""M12,80.75c2.37,0.5,6.73,0.67,9.09,0.5c23.79-1.75,45.04-4.12,67.49-4.74c3.95-0.11,6.32,0.24,8.3,0.49""/>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_04e8c"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 17.50 33.13)"">1</text>
	<text transform=""matrix(1 0 0 1 3.50 81.50)"">2</text>
</g>
</svg>
",
                OrderIndex = 2,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("08c12d3c-0774-57c9-9fb3-a3ce7b07d4c8"),
                LessonId = lesson2Id,
                Level = "N5",
                Character = "三",
                HanViet = "TAM",
                Meaning = "Số ba",
                StrokeCount = 3,
                KunReading = "みっ.つ",
                OnReading = "サン",
                Mnemonic = "Ba nét ngang kích thước khác nhau xếp chồng lên nhau, biểu thị số ba (三).",
                ComponentMapJson = @"[]",
                StrokeDataJson = @"[""M27.5,23.65c3.09,0.73,6.29,0.36,9.4,0.06c10.2-1,27-2.94,38.97-3.57c3.06-0.16,6.09-0.2,9.14,0.23"", ""M28.75,55.14c3.13,0.76,6.46,0.43,9.64,0.2c10.03-0.72,23.97-2.63,34.73-3.12c2.7-0.12,5.45-0.16,8.13,0.3"", ""M13,87.83c3.94,1.01,7.72,0.96,11.75,0.72c18.41-1.07,41.27-3.39,61.12-4.07c3.63-0.13,7.2-0.1,10.75,0.78""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_04e09"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:04e09"" kvg:element=""三"">
	<g id=""kvg:04e09-g1"" kvg:element=""一"" kvg:position=""top"" kvg:radical=""general"">
		<path id=""kvg:04e09-s1"" kvg:type=""㇐"" d=""M27.5,23.65c3.09,0.73,6.29,0.36,9.4,0.06c10.2-1,27-2.94,38.97-3.57c3.06-0.16,6.09-0.2,9.14,0.23""/>
	</g>
	<g id=""kvg:04e09-g2"" kvg:position=""bottom"">
		<g id=""kvg:04e09-g3"" kvg:element=""一"">
			<path id=""kvg:04e09-s2"" kvg:type=""㇐"" d=""M28.75,55.14c3.13,0.76,6.46,0.43,9.64,0.2c10.03-0.72,23.97-2.63,34.73-3.12c2.7-0.12,5.45-0.16,8.13,0.3""/>
		</g>
		<g id=""kvg:04e09-g4"" kvg:element=""一"">
			<path id=""kvg:04e09-s3"" kvg:type=""㇐"" d=""M13,87.83c3.94,1.01,7.72,0.96,11.75,0.72c18.41-1.07,41.27-3.39,61.12-4.07c3.63-0.13,7.2-0.1,10.75,0.78""/>
		</g>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_04e09"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 18.00 25.78)"">1</text>
	<text transform=""matrix(1 0 0 1 18.75 57.13)"">2</text>
	<text transform=""matrix(1 0 0 1 3.75 91.63)"">3</text>
</g>
</svg>
",
                OrderIndex = 3,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("5281ba82-3a72-5b09-960c-017fcda7e538"),
                LessonId = lesson2Id,
                Level = "N5",
                Character = "四",
                HanViet = "TỨ",
                Meaning = "Số bốn",
                StrokeCount = 5,
                KunReading = "よん、よっ.つ",
                OnReading = "シ",
                Mnemonic = "Trong một khu vực kín (囗), con số bốn (四) như có hai cái chân (儿) đang tung tăng nhảy múa.",
                ComponentMapJson = @"[{""character"": ""囗"", ""component"": ""囗"", ""name"": ""囗"", ""meaning"": ""囗""}, {""character"": ""儿"", ""component"": ""儿"", ""name"": ""儿"", ""meaning"": ""儿""}]",
                StrokeDataJson = @"[""M14.5,31.48c1.51,1.51,2.25,3.27,2.53,5.2c1.14,7.9,2.61,25.18,4.39,40.83c0.29,2.55,0.34,3.81,0.64,6.24"", ""M17.85,34.04c21.65-1.92,51.52-3.92,67.82-4.3c4.85-0.11,6.31,2.62,6.04,5.38c-0.9,9.02-4.17,28.29-6.41,39.62c-0.49,2.49-0.94,4.6-1.3,6.13"", ""M40.5,36c0.08,0.64,0.12,1.65-0.16,2.57c-2.22,7.3-5.1,14.55-13.35,22.68"", ""M59.75,34.25c0.8,1.05,1.44,2.29,1.49,3.92c0.11,3.62,0.05,7.05,0.05,9.89c0,6.94,0.71,7.54,9.47,7.54c4.99,0,8.86-0.72,10.25-1.72"", ""M22.73,79.32c13.77-0.57,43.64-1.8,61.18-2.08""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_056db"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:056db"" kvg:element=""四"">
	<g id=""kvg:056db-g1"" kvg:element=""囗"" kvg:part=""1"" kvg:position=""kamae"" kvg:radical=""general"">
		<path id=""kvg:056db-s1"" kvg:type=""㇑"" d=""M14.5,31.48c1.51,1.51,2.25,3.27,2.53,5.2c1.14,7.9,2.61,25.18,4.39,40.83c0.29,2.55,0.34,3.81,0.64,6.24""/>
		<path id=""kvg:056db-s2"" kvg:type=""㇕a"" d=""M17.85,34.04c21.65-1.92,51.52-3.92,67.82-4.3c4.85-0.11,6.31,2.62,6.04,5.38c-0.9,9.02-4.17,28.29-6.41,39.62c-0.49,2.49-0.94,4.6-1.3,6.13""/>
	</g>
	<g id=""kvg:056db-g2"" kvg:element=""儿"" kvg:original=""八"">
		<g id=""kvg:056db-g3"" kvg:element=""丿"">
			<path id=""kvg:056db-s3"" kvg:type=""㇒"" d=""M40.5,36c0.08,0.64,0.12,1.65-0.16,2.57c-2.22,7.3-5.1,14.55-13.35,22.68""/>
		</g>
		<path id=""kvg:056db-s4"" kvg:type=""㇟a"" d=""M59.75,34.25c0.8,1.05,1.44,2.29,1.49,3.92c0.11,3.62,0.05,7.05,0.05,9.89c0,6.94,0.71,7.54,9.47,7.54c4.99,0,8.86-0.72,10.25-1.72""/>
	</g>
	<g id=""kvg:056db-g4"" kvg:element=""囗"" kvg:part=""2"" kvg:position=""kamae"" kvg:radical=""general"">
		<path id=""kvg:056db-s5"" kvg:type=""㇐a"" d=""M22.73,79.32c13.77-0.57,43.64-1.8,61.18-2.08""/>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_056db"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 8.75 40.63)"">1</text>
	<text transform=""matrix(1 0 0 1 20.75 29.13)"">2</text>
	<text transform=""matrix(1 0 0 1 31.25 41.63)"">3</text>
	<text transform=""matrix(1 0 0 1 52.25 40.13)"">4</text>
	<text transform=""matrix(1 0 0 1 27.25 75.63)"">5</text>
</g>
</svg>
",
                OrderIndex = 4,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("3157f1ba-0189-5b6e-9ffd-4626d4bac967"),
                LessonId = lesson2Id,
                Level = "N5",
                Character = "五",
                HanViet = "NGŨ",
                Meaning = "Số năm",
                StrokeCount = 4,
                KunReading = "いつ.つ",
                OnReading = "ゴ",
                Mnemonic = "Trông giống như một cái nắp hộp bị méo mó, biến dạng tạo thành hình số năm (五).",
                ComponentMapJson = @"[]",
                StrokeDataJson = @"[""M31.75,23.15c2.8,0.67,5.54,0.42,8.36,0.12c9.3-0.99,22.18-2.4,34.14-3.21c2.49-0.17,5.04-0.33,7.5,0.2"", ""M55.75,25.25c0.62,1.25,1.02,3.01,0.5,5c-3.12,11.88-14,44.12-19.75,59"", ""M25.5,55.25c2.07,1.24,4.73,1.03,7,0.81c15.49-1.45,29.89-3.03,42.25-4.06c3-0.25,4.25,1.75,3.5,3.75c-2.24,5.96-6,20.75-7.75,31.5"", ""M11.25,90.5c3.04,0.81,6.52,0.63,9.63,0.41c15.71-1.1,43.9-2.8,67.75-3.8c3.41-0.14,6.9-0.4,10.25,0.39""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_04e94"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:04e94"" kvg:element=""五"">
	<g id=""kvg:04e94-g1"" kvg:element=""二"" kvg:part=""1"" kvg:radical=""tradit"">
		<g id=""kvg:04e94-g2"" kvg:element=""一"" kvg:radical=""nelson"">
			<path id=""kvg:04e94-s1"" kvg:type=""㇐"" d=""M31.75,23.15c2.8,0.67,5.54,0.42,8.36,0.12c9.3-0.99,22.18-2.4,34.14-3.21c2.49-0.17,5.04-0.33,7.5,0.2""/>
		</g>
	</g>
	<path id=""kvg:04e94-s2"" kvg:type=""㇑a"" d=""M55.75,25.25c0.62,1.25,1.02,3.01,0.5,5c-3.12,11.88-14,44.12-19.75,59""/>
	<path id=""kvg:04e94-s3"" kvg:type=""㇕c"" d=""M25.5,55.25c2.07,1.24,4.73,1.03,7,0.81c15.49-1.45,29.89-3.03,42.25-4.06c3-0.25,4.25,1.75,3.5,3.75c-2.24,5.96-6,20.75-7.75,31.5""/>
	<g id=""kvg:04e94-g3"" kvg:element=""二"" kvg:part=""2"" kvg:radical=""tradit"">
		<path id=""kvg:04e94-s4"" kvg:type=""㇐"" d=""M11.25,90.5c3.04,0.81,6.52,0.63,9.63,0.41c15.71-1.1,43.9-2.8,67.75-3.8c3.41-0.14,6.9-0.4,10.25,0.39""/>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_04e94"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 24.50 24.13)"">1</text>
	<text transform=""matrix(1 0 0 1 47.50 33.50)"">2</text>
	<text transform=""matrix(1 0 0 1 18.50 57.50)"">3</text>
	<text transform=""matrix(1 0 0 1 3.50 91.50)"">4</text>
</g>
</svg>
",
                OrderIndex = 5,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("e7009d91-c503-5e4c-b319-6b7d917fa37f"),
                LessonId = lesson2Id,
                Level = "N5",
                Character = "六",
                HanViet = "LỤC",
                Meaning = "Số sáu",
                StrokeCount = 4,
                KunReading = "むっ.つ",
                OnReading = "ロク",
                Mnemonic = "Một cái đầu (亠) to nhưng chỉ mọc lơ thơ có tám (ハ) trừ hai bằng sáu (六) sợi tóc.",
                ComponentMapJson = @"[{""character"": ""亠"", ""component"": ""亠"", ""name"": ""亠"", ""meaning"": ""亠""}]",
                StrokeDataJson = @"[""M51.87,17.5c1.78,1.78,2.71,3.48,2.71,6.5c0,6.46,0.12,9.16,0.12,14.35"", ""M13.5,42.13c3.27,0.74,7.11,0.89,9.93,0.64c21.56-1.9,41.78-5.02,61.41-5.47c4.8-0.11,7.49,0.31,11.06,1.07"", ""M38.11,58.6c0.51,1.37,0.42,3.67-0.49,5.29C33.38,71.38,24,82.38,15.41,88.75"", ""M70.16,59.92c9.96,8.61,18.18,18.54,23.16,28.99""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_0516d"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:0516d"" kvg:element=""六"">
	<g id=""kvg:0516d-g1"" kvg:element=""亠"" kvg:position=""top"" kvg:radical=""nelson"">
		<path id=""kvg:0516d-s1"" kvg:type=""㇑a"" d=""M51.87,17.5c1.78,1.78,2.71,3.48,2.71,6.5c0,6.46,0.12,9.16,0.12,14.35""/>
		<path id=""kvg:0516d-s2"" kvg:type=""㇐"" d=""M13.5,42.13c3.27,0.74,7.11,0.89,9.93,0.64c21.56-1.9,41.78-5.02,61.41-5.47c4.8-0.11,7.49,0.31,11.06,1.07""/>
	</g>
	<g id=""kvg:0516d-g2"" kvg:element=""八"" kvg:position=""bottom"" kvg:radical=""tradit"">
		<path id=""kvg:0516d-s3"" kvg:type=""㇒"" d=""M38.11,58.6c0.51,1.37,0.42,3.67-0.49,5.29C33.38,71.38,24,82.38,15.41,88.75""/>
		<path id=""kvg:0516d-s4"" kvg:type=""㇔/㇏"" d=""M70.16,59.92c9.96,8.61,18.18,18.54,23.16,28.99""/>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_0516d"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 42.25 17.63)"">1</text>
	<text transform=""matrix(1 0 0 1 5.50 43.63)"">2</text>
	<text transform=""matrix(1 0 0 1 27.50 58.63)"">3</text>
	<text transform=""matrix(1 0 0 1 60.50 58.63)"">4</text>
</g>
</svg>
",
                OrderIndex = 6,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("4ac63e13-411a-5b19-a116-c6778cabcc8b"),
                LessonId = lesson2Id,
                Level = "N5",
                Character = "七",
                HanViet = "THẤT",
                Meaning = "Số bảy",
                StrokeCount = 2,
                KunReading = "なな、なな.つ",
                OnReading = "シチ",
                Mnemonic = "Chữ này nhìn y hệt con số 7 nhưng bị lật ngược lại.",
                ComponentMapJson = @"[]",
                StrokeDataJson = @"[""M15.5,51.75c1.82,0.5,4.38,0.88,6.96,0.5c16.91-2.45,50.92-8.12,64.44-8.74c3.02-0.14,4.84,0.24,6.35,0.49"", ""M43,20c1.38,1.38,2.15,3.25,2.15,5.26C45.15,29.5,45,71.84,45,76c0,10.5,2.25,12.25,20.25,12.25c18.75,0,20-3.75,20-2.75""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_04e03"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:04e03"" kvg:element=""七"">
	<g id=""kvg:04e03-g1"" kvg:element=""一"" kvg:radical=""tradit"">
		<path id=""kvg:04e03-s1"" kvg:type=""㇐"" d=""M15.5,51.75c1.82,0.5,4.38,0.88,6.96,0.5c16.91-2.45,50.92-8.12,64.44-8.74c3.02-0.14,4.84,0.24,6.35,0.49""/>
	</g>
	<g id=""kvg:04e03-g2"" kvg:element=""乙"" kvg:variant=""true"" kvg:radical=""nelson"">
		<path id=""kvg:04e03-s2"" kvg:type=""㇄"" d=""M43,20c1.38,1.38,2.15,3.25,2.15,5.26C45.15,29.5,45,71.84,45,76c0,10.5,2.25,12.25,20.25,12.25c18.75,0,20-3.75,20-2.75""/>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_04e03"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 8.50 53.50)"">1</text>
	<text transform=""matrix(1 0 0 1 34.50 19.63)"">2</text>
</g>
</svg>
",
                OrderIndex = 7,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("ee9d9eb2-e9e3-56d0-8676-47ed4b603367"),
                LessonId = lesson2Id,
                Level = "N5",
                Character = "八",
                HanViet = "BÁT",
                Meaning = "Số tám",
                StrokeCount = 2,
                KunReading = "やって.つ",
                OnReading = "ハチ",
                Mnemonic = "Hai nét phẩy xòe rộng tách chẻ ra hai bên, giống như đang chia đều chiếc bánh ra thành tám (八) phần.",
                ComponentMapJson = @"[]",
                StrokeDataJson = @"[""M37.22,45c0.28,1.5,0.2,3.21-0.86,5.48c-4.23,9.02-11.48,20.4-24.1,32.02"", ""M48,27.25c9.38,0.25,21.12,30,37.27,45.72c3.79,3.69,6.73,5.66,9.98,7.03""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_0516b"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:0516b"" kvg:element=""八"" kvg:radical=""general"">
	<g id=""kvg:0516b-g1"" kvg:position=""left"">
		<path id=""kvg:0516b-s1"" kvg:type=""㇒"" d=""M37.22,45c0.28,1.5,0.2,3.21-0.86,5.48c-4.23,9.02-11.48,20.4-24.1,32.02""/>
	</g>
	<g id=""kvg:0516b-g2"" kvg:position=""right"">
		<path id=""kvg:0516b-s2"" kvg:type=""㇏"" d=""M48,27.25c9.38,0.25,21.12,30,37.27,45.72c3.79,3.69,6.73,5.66,9.98,7.03""/>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_0516b"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 27.75 42.13)"">1</text>
	<text transform=""matrix(1 0 0 1 51.75 22.63)"">2</text>
</g>
</svg>
",
                OrderIndex = 8,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("baf49199-9e6c-5f39-b696-c1ae140ebb7b"),
                LessonId = lesson2Id,
                Level = "N5",
                Character = "九",
                HanViet = "CỬU",
                Meaning = "Số chín",
                StrokeCount = 2,
                KunReading = "ここの.つ",
                OnReading = "キュウ、ク",
                Mnemonic = "Một người đang hít đất và gập cong người lại, tạo thành hình số chín (九).",
                ComponentMapJson = @"[]",
                StrokeDataJson = @"[""M41.88,14.38c1,1.38,1.5,3.25,1.5,5.12c0,40.13-9.12,57.5-28.5,68.75"", ""M13.5,45.75c2.88,0.85,5.78,0.05,8.58-0.66c8.47-2.14,39.88-9.79,40.92-9.84c2.5-0.12,4.75,0.5,4.25,4.75c-0.5,4.25-5.5,20.75-7,32.5c-2.23,17.46,2,19.37,18.21,19.37c13.79,0,19.01-1.07,19.27-10.12""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_04e5d"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:04e5d"" kvg:element=""九"">
	<g id=""kvg:04e5d-g1"" kvg:element=""丿"" kvg:radical=""nelson"">
		<path id=""kvg:04e5d-s1"" kvg:type=""㇒"" d=""M41.88,14.38c1,1.38,1.5,3.25,1.5,5.12c0,40.13-9.12,57.5-28.5,68.75""/>
	</g>
	<g id=""kvg:04e5d-g2"" kvg:element=""乙"" kvg:radical=""tradit"">
		<path id=""kvg:04e5d-s2"" kvg:type=""㇈"" d=""M13.5,45.75c2.88,0.85,5.78,0.05,8.58-0.66c8.47-2.14,39.88-9.79,40.92-9.84c2.5-0.12,4.75,0.5,4.25,4.75c-0.5,4.25-5.5,20.75-7,32.5c-2.23,17.46,2,19.37,18.21,19.37c13.79,0,19.01-1.07,19.27-10.12""/>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_04e5d"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 32.50 14.50)"">1</text>
	<text transform=""matrix(1 0 0 1 5.50 46.63)"">2</text>
</g>
</svg>
",
                OrderIndex = 9,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("ddb5d53b-5213-599d-b9be-3536802be0e7"),
                LessonId = lesson2Id,
                Level = "N5",
                Character = "十",
                HanViet = "THẬP",
                Meaning = "Số mười",
                StrokeCount = 2,
                KunReading = "とお",
                OnReading = "ジュウ",
                Mnemonic = "Hai nét gạch chéo tạo thành hình chữ thập, biểu thị sự trọn vẹn, đầy đủ của số mười (十).",
                ComponentMapJson = @"[]",
                StrokeDataJson = @"[""M11.88,50.98c3.18,0.89,6.62,0.61,9.87,0.35c19.92-1.58,45.23-4.76,63.38-5.82c3.85-0.23,7.23-0.07,11,0.56"", ""M52.22,11.63c1.4,1.4,2.2,3.96,2.2,6.26c0,1.13-0.03,51.22-0.19,73.41c-0.03,3.96-0.06,6.83-0.08,8.08""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_05341"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:05341"" kvg:element=""十"" kvg:radical=""general"">
	<path id=""kvg:05341-s1"" kvg:type=""㇐"" d=""M11.88,50.98c3.18,0.89,6.62,0.61,9.87,0.35c19.92-1.58,45.23-4.76,63.38-5.82c3.85-0.23,7.23-0.07,11,0.56""/>
	<path id=""kvg:05341-s2"" kvg:type=""㇑"" d=""M52.22,11.63c1.4,1.4,2.2,3.96,2.2,6.26c0,1.13-0.03,51.22-0.19,73.41c-0.03,3.96-0.06,6.83-0.08,8.08""/>
</g>
</g>
<g id=""kvg:StrokeNumbers_05341"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 5.50 52.50)"">1</text>
	<text transform=""matrix(1 0 0 1 42.75 12.50)"">2</text>
</g>
</svg>
",
                OrderIndex = 10,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("5b211445-142d-507e-9963-47430a88e47b"),
                LessonId = lesson2Id,
                Level = "N5",
                Character = "百",
                HanViet = "BÁCH",
                Meaning = "Một trăm",
                StrokeCount = 6,
                KunReading = "",
                OnReading = "ヒャク",
                Mnemonic = "Đạt được một (一) tờ giấy trắng (白) tinh khôi không tì vết là biểu tượng của một trăm (百) điểm tuyệt đối.",
                ComponentMapJson = @"[{""character"": ""一"", ""component"": ""一"", ""name"": ""一"", ""meaning"": ""一""}, {""character"": ""白"", ""component"": ""白"", ""name"": ""白"", ""meaning"": ""白""}]",
                StrokeDataJson = @"[""M16.13,20.23c2.22,0.54,6.29,0.75,8.51,0.54c21.49-2.02,41.86-4.39,59.22-4.98c3.7-0.12,5.92,0.26,7.77,0.53"", ""M52.31,21.75c0.19,1.38,0.19,2.5-0.38,3.93c-1.65,4.19-4.81,9.19-8.66,14.68"", ""M30.75,42.82c0.96,0.96,1.64,2.45,1.72,4.19c0.41,8.74,0.96,32.92,1.18,43.74c0.05,2.48,0.08,4.12,0.1,4.5"", ""M33.55,44.8c10.35-1.37,35.73-4.38,38.78-4.59c3.15-0.22,4.92,1.17,4.92,4.24c0,4.48-0.68,32-0.92,44.06c-0.06,3.02-0.1,5.05-0.11,5.48"", ""M34.14,66.95c10.24-1.08,32.11-3.2,41.44-3.57"", ""M34.97,92.87c8.78-0.87,30.53-2.12,40.06-2.39""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_0767e"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:0767e"" kvg:element=""百"">
	<g id=""kvg:0767e-g1"" kvg:element=""一"" kvg:position=""top"" kvg:radical=""nelson"">
		<path id=""kvg:0767e-s1"" kvg:type=""㇐"" d=""M16.13,20.23c2.22,0.54,6.29,0.75,8.51,0.54c21.49-2.02,41.86-4.39,59.22-4.98c3.7-0.12,5.92,0.26,7.77,0.53""/>
	</g>
	<g id=""kvg:0767e-g2"" kvg:element=""白"" kvg:position=""bottom"" kvg:radical=""tradit"">
		<g id=""kvg:0767e-g3"" kvg:position=""top"">
			<path id=""kvg:0767e-s2"" kvg:type=""㇔"" d=""M52.31,21.75c0.19,1.38,0.19,2.5-0.38,3.93c-1.65,4.19-4.81,9.19-8.66,14.68""/>
		</g>
		<g id=""kvg:0767e-g4"" kvg:element=""日"" kvg:position=""bottom"">
			<path id=""kvg:0767e-s3"" kvg:type=""㇑"" d=""M30.75,42.82c0.96,0.96,1.64,2.45,1.72,4.19c0.41,8.74,0.96,32.92,1.18,43.74c0.05,2.48,0.08,4.12,0.1,4.5""/>
			<path id=""kvg:0767e-s4"" kvg:type=""㇕a"" d=""M33.55,44.8c10.35-1.37,35.73-4.38,38.78-4.59c3.15-0.22,4.92,1.17,4.92,4.24c0,4.48-0.68,32-0.92,44.06c-0.06,3.02-0.1,5.05-0.11,5.48""/>
			<path id=""kvg:0767e-s5"" kvg:type=""㇐a"" d=""M34.14,66.95c10.24-1.08,32.11-3.2,41.44-3.57""/>
			<path id=""kvg:0767e-s6"" kvg:type=""㇐a"" d=""M34.97,92.87c8.78-0.87,30.53-2.12,40.06-2.39""/>
		</g>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_0767e"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 8.25 21.50)"">1</text>
	<text transform=""matrix(1 0 0 1 42.50 28.63)"">2</text>
	<text transform=""matrix(1 0 0 1 23.50 51.50)"">3</text>
	<text transform=""matrix(1 0 0 1 33.50 41.50)"">4</text>
	<text transform=""matrix(1 0 0 1 37.50 63.10)"">5</text>
	<text transform=""matrix(1 0 0 1 37.50 89.50)"">6</text>
</g>
</svg>
",
                OrderIndex = 11,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("7e864a3b-ca27-5a05-abd8-a6c16639a2b5"),
                LessonId = lesson2Id,
                Level = "N5",
                Character = "千",
                HanViet = "THIÊN",
                Meaning = "Một nghìn",
                StrokeCount = 3,
                KunReading = "ち",
                OnReading = "セン",
                Mnemonic = "Chỉ cần thêm một nét phẩy (ノ) vào trên đầu số mười (十), giá trị lập tức tăng lên gấp trăm lần thành một nghìn (千).",
                ComponentMapJson = @"[{""character"": ""十"", ""component"": ""十"", ""name"": ""十"", ""meaning"": ""十""}]",
                StrokeDataJson = @"[""M70.38,10.17c-0.13,1.58-0.83,2.64-2.17,3.67c-5.71,4.41-21.46,11.91-41.57,16.82"", ""M12.13,50.83c3.36,0.94,7.21,0.75,10.63,0.49c17.76-1.34,37.63-4.16,66.24-4.94c3.08-0.08,6.08-0.14,9.13,0.38"", ""M54.56,25.25c1.03,1.03,2.01,3,2.01,5.18c0,0.9-0.07,46.38-0.19,63.58c-0.02,2.93-0.04,5.04-0.06,5.99""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_05343"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:05343"" kvg:element=""千"">
	<g id=""kvg:05343-g1"" kvg:element=""丿"" kvg:position=""top"" kvg:radical=""nelson"">
		<path id=""kvg:05343-s1"" kvg:type=""㇒"" d=""M70.38,10.17c-0.13,1.58-0.83,2.64-2.17,3.67c-5.71,4.41-21.46,11.91-41.57,16.82""/>
	</g>
	<g id=""kvg:05343-g2"" kvg:element=""十"" kvg:position=""bottom"" kvg:radical=""tradit"">
		<path id=""kvg:05343-s2"" kvg:type=""㇐"" d=""M12.13,50.83c3.36,0.94,7.21,0.75,10.63,0.49c17.76-1.34,37.63-4.16,66.24-4.94c3.08-0.08,6.08-0.14,9.13,0.38""/>
		<path id=""kvg:05343-s3"" kvg:type=""㇑"" d=""M54.56,25.25c1.03,1.03,2.01,3,2.01,5.18c0,0.9-0.07,46.38-0.19,63.58c-0.02,2.93-0.04,5.04-0.06,5.99""/>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_05343"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 61.75 8.13)"">1</text>
	<text transform=""matrix(1 0 0 1 5.25 52.63)"">2</text>
	<text transform=""matrix(1 0 0 1 48.50 35.50)"">3</text>
</g>
</svg>
",
                OrderIndex = 12,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("9270b873-9f8c-55cc-abfb-d278201b7001"),
                LessonId = lesson2Id,
                Level = "N5",
                Character = "万",
                HanViet = "VẠN",
                Meaning = "Mười nghìn",
                StrokeCount = 3,
                KunReading = "",
                OnReading = "マン、バン",
                Mnemonic = "Một (一) vòng tay dang rộng bao bọc lấy hàng vạn (万) sinh linh bé nhỏ.",
                ComponentMapJson = @"[{""character"": ""一"", ""component"": ""一"", ""name"": ""一"", ""meaning"": ""一""}]",
                StrokeDataJson = @"[""M14.38,24.73c2.3,0.54,6.52,0.78,8.81,0.54c21.57-2.27,44.44-5.64,64.9-5.98c3.83-0.06,6.12,0.26,8.04,0.53"", ""M51,41.5c1.45,0.7,3.19,1.43,5.19,1.74c7.31,1.14,17.05,1.94,22.64,1.5c4.64-0.37,6.38,1.08,5.17,4.73C77.88,68,72.75,78.75,63.87,90.4c-7.6,9.97-10.12,3.22-12.62,0.2"", ""M51.75,25.5c0.5,2,0.22,3.78-0.21,5.89C48.95,43.8,34.75,73.38,13.56,87.97""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_04e07"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:04e07"" kvg:element=""万"">
	<g id=""kvg:04e07-g1"" kvg:element=""一"" kvg:radical=""general"">
		<path id=""kvg:04e07-s1"" kvg:type=""㇐"" d=""M14.38,24.73c2.3,0.54,6.52,0.78,8.81,0.54c21.57-2.27,44.44-5.64,64.9-5.98c3.83-0.06,6.12,0.26,8.04,0.53""/>
	</g>
	<path id=""kvg:04e07-s2"" kvg:type=""㇆"" d=""M51,41.5c1.45,0.7,3.19,1.43,5.19,1.74c7.31,1.14,17.05,1.94,22.64,1.5c4.64-0.37,6.38,1.08,5.17,4.73C77.88,68,72.75,78.75,63.87,90.4c-7.6,9.97-10.12,3.22-12.62,0.2""/>
	<path id=""kvg:04e07-s3"" kvg:type=""㇒"" d=""M51.75,25.5c0.5,2,0.22,3.78-0.21,5.89C48.95,43.8,34.75,73.38,13.56,87.97""/>
</g>
</g>
<g id=""kvg:StrokeNumbers_04e07"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 7.50 26.50)"">1</text>
	<text transform=""matrix(1 0 0 1 57.50 41.50)"">2</text>
	<text transform=""matrix(1 0 0 1 43.50 33.50)"">3</text>
</g>
</svg>
",
                OrderIndex = 13,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("c0154f99-b8b8-557c-bb1f-0497014dadfb"),
                LessonId = lesson2Id,
                Level = "N5",
                Character = "円",
                HanViet = "VIÊN",
                Meaning = "Hình tròn, đồng Yên",
                StrokeCount = 4,
                KunReading = "まる.い",
                OnReading = "エン",
                Mnemonic = "Nhìn qua cái khung cửa sổ (冂), thấy bên trong có những thanh vàng xếp ngang dọc để đổi thành đồng Yên (円) Nhật.",
                ComponentMapJson = @"[{""character"": ""冂"", ""component"": ""冂"", ""name"": ""冂"", ""meaning"": ""冂""}]",
                StrokeDataJson = @"[""M21.75,19.8c0.91,0.91,1.47,3.23,1.5,5.45c0.2,13.9,0.03,47.69,0.03,62.5c0,2-0.03,4.99-0.03,6"", ""M24.06,21.56c15.07-1.68,49.46-5.58,57.92-6.31c2.9-0.25,4.78,1.88,4.78,4.27c0,13.48,0,53.21,0,67.48c0,9.75-4.25,6.5-8.5,1.5"", ""M52.25,20.75c0.88,0.88,1.5,2,1.5,3.71c0,6.76,0,27.54,0,31.04"", ""M24.75,59.75c14.62-1.75,43-4.25,60.5-5.25""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_05186"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:05186"" kvg:element=""円"">
	<g id=""kvg:05186-g1"" kvg:element=""冂"" kvg:radical=""general"">
		<path id=""kvg:05186-s1"" kvg:type=""㇑"" d=""M21.75,19.8c0.91,0.91,1.47,3.23,1.5,5.45c0.2,13.9,0.03,47.69,0.03,62.5c0,2-0.03,4.99-0.03,6""/>
		<path id=""kvg:05186-s2"" kvg:type=""㇆a"" d=""M24.06,21.56c15.07-1.68,49.46-5.58,57.92-6.31c2.9-0.25,4.78,1.88,4.78,4.27c0,13.48,0,53.21,0,67.48c0,9.75-4.25,6.5-8.5,1.5""/>
	</g>
	<g id=""kvg:05186-g2"" kvg:phon=""員V"">
		<path id=""kvg:05186-s3"" kvg:type=""㇑a"" d=""M52.25,20.75c0.88,0.88,1.5,2,1.5,3.71c0,6.76,0,27.54,0,31.04""/>
		<path id=""kvg:05186-s4"" kvg:type=""㇐a"" d=""M24.75,59.75c14.62-1.75,43-4.25,60.5-5.25""/>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_05186"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 16.50 28.63)"">1</text>
	<text transform=""matrix(1 0 0 1 24.75 17.50)"">2</text>
	<text transform=""matrix(1 0 0 1 45.50 29.50)"">3</text>
	<text transform=""matrix(1 0 0 1 28.50 56.50)"">4</text>
</g>
</svg>
",
                OrderIndex = 14,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
        };

        foreach (var item in kanjiItems2)
        {
            if (!await db.KanjiItems.AnyAsync(k => k.Character == item.Character))
                db.KanjiItems.Add(item);
        }

        var vocabItems2 = new List<KanjiVocabulary>
        {
            new KanjiVocabulary {
                Id = Guid.Parse("c02f132b-5453-55ed-9663-f1c879ee6d19"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("f67923c2-4634-53f4-8606-fcc50c8de6c4"),
                Level = "N5",
                Word = "一日",
                Reading = "ついたち",
                Meaning = "Ngày mồng 1",
                OrderIndex = 1,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("1d427a64-eb02-559d-b8c5-b3c0cab0f31a"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("f67923c2-4634-53f4-8606-fcc50c8de6c4"),
                Level = "N5",
                Word = "一月",
                Reading = "いちがつ",
                Meaning = "tháng 1",
                OrderIndex = 2,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("9a74e688-ab52-5520-b253-104ec75e4c53"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("f67923c2-4634-53f4-8606-fcc50c8de6c4"),
                Level = "N5",
                Word = "一人",
                Reading = "ひとり",
                Meaning = "1 người",
                OrderIndex = 3,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("6975a269-95cb-584d-b5af-896a1a6d1b54"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("f67923c2-4634-53f4-8606-fcc50c8de6c4"),
                Level = "N5",
                Word = "一つ",
                Reading = "ひとつ",
                Meaning = "1 chiếc",
                OrderIndex = 4,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("76a13148-f6c6-5735-a295-100fa3e69955"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("f67923c2-4634-53f4-8606-fcc50c8de6c4"),
                Level = "N5",
                Word = "一",
                Reading = "いち",
                Meaning = "Số 1",
                OrderIndex = 5,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("b5e67006-ce50-5073-9a5f-5267eab523cb"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("472283d7-6919-56ff-9fec-2d6aa36cc077"),
                Level = "N5",
                Word = "二日",
                Reading = "ふつか",
                Meaning = "Ngày mồng 2",
                OrderIndex = 6,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("1a97650f-b776-54a3-926a-544e202cb128"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("472283d7-6919-56ff-9fec-2d6aa36cc077"),
                Level = "N5",
                Word = "二月",
                Reading = "にがつ",
                Meaning = "Tháng 2",
                OrderIndex = 7,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("7a05ad0c-3fa0-5b02-afb1-667835bf6be7"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("472283d7-6919-56ff-9fec-2d6aa36cc077"),
                Level = "N5",
                Word = "二つ",
                Reading = "ふたつ",
                Meaning = "2 cái",
                OrderIndex = 8,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("99f20241-3623-52d0-b16b-69fde006c7da"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("472283d7-6919-56ff-9fec-2d6aa36cc077"),
                Level = "N5",
                Word = "二",
                Reading = "に",
                Meaning = "Số 2",
                OrderIndex = 9,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("eda0dc35-b4c5-55dc-9bb3-bc108947a2bf"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("08c12d3c-0774-57c9-9fb3-a3ce7b07d4c8"),
                Level = "N5",
                Word = "三日",
                Reading = "みっか",
                Meaning = "Ngày mồng 3",
                OrderIndex = 10,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("95b90564-778b-51bf-8fbb-35a49afb2247"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("08c12d3c-0774-57c9-9fb3-a3ce7b07d4c8"),
                Level = "N5",
                Word = "三月",
                Reading = "さんがつ",
                Meaning = "tháng 3",
                OrderIndex = 11,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("1eb74bfd-3f6e-58f4-a144-be02ab2a6448"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("08c12d3c-0774-57c9-9fb3-a3ce7b07d4c8"),
                Level = "N5",
                Word = "三人",
                Reading = "さんにん",
                Meaning = "3 người",
                OrderIndex = 12,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("d7e1f0e2-28d4-51e8-a601-934bc5cf3b68"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("08c12d3c-0774-57c9-9fb3-a3ce7b07d4c8"),
                Level = "N5",
                Word = "三",
                Reading = "さん",
                Meaning = "Số 3",
                OrderIndex = 13,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("4bde79ce-c4d9-5093-89c3-4065a03fdf40"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("5281ba82-3a72-5b09-960c-017fcda7e538"),
                Level = "N5",
                Word = "四日",
                Reading = "よっか",
                Meaning = "Ngày mồng 4",
                OrderIndex = 14,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("8ad8377e-b5e2-5679-9c46-a85d5ae1cf47"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("5281ba82-3a72-5b09-960c-017fcda7e538"),
                Level = "N5",
                Word = "四月",
                Reading = "しがつ",
                Meaning = "tháng 4",
                OrderIndex = 15,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("80e86aae-ab69-5a58-800e-0330608dcdac"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("5281ba82-3a72-5b09-960c-017fcda7e538"),
                Level = "N5",
                Word = "四人",
                Reading = "よにん",
                Meaning = "4 người",
                OrderIndex = 16,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("a4648fde-583c-539e-9dd1-6fd695663282"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("5281ba82-3a72-5b09-960c-017fcda7e538"),
                Level = "N5",
                Word = "四",
                Reading = "よん",
                Meaning = "Số 4",
                OrderIndex = 17,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("c968d2eb-8bb4-5bcf-9cd5-974ff2d45a7a"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("3157f1ba-0189-5b6e-9ffd-4626d4bac967"),
                Level = "N5",
                Word = "五日",
                Reading = "いつか",
                Meaning = "Ngày mồng 5",
                OrderIndex = 18,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("15f1a6cd-674f-5628-aef7-e65311e8a610"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("3157f1ba-0189-5b6e-9ffd-4626d4bac967"),
                Level = "N5",
                Word = "五月",
                Reading = "ごがつ",
                Meaning = "tháng 5",
                OrderIndex = 19,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("22f86824-eff5-5cf8-a27c-3d602f3c8853"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("3157f1ba-0189-5b6e-9ffd-4626d4bac967"),
                Level = "N5",
                Word = "五つ",
                Reading = "いつつ",
                Meaning = "5 cái",
                OrderIndex = 20,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("eb2567ee-b289-5097-b9c0-0b55e3fe630e"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("3157f1ba-0189-5b6e-9ffd-4626d4bac967"),
                Level = "N5",
                Word = "五",
                Reading = "ご",
                Meaning = "Số 5",
                OrderIndex = 21,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("52955b18-4d87-56a0-87b6-4a0f9d996171"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("e7009d91-c503-5e4c-b319-6b7d917fa37f"),
                Level = "N5",
                Word = "六日",
                Reading = "むいか",
                Meaning = "Ngày mồng 6",
                OrderIndex = 22,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("e03340bf-48a7-5ead-a197-666a75a26a68"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("e7009d91-c503-5e4c-b319-6b7d917fa37f"),
                Level = "N5",
                Word = "六月",
                Reading = "ろくがつ",
                Meaning = "tháng 6",
                OrderIndex = 23,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("4aa4d5bd-2526-5550-ae23-908645dcef5a"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("e7009d91-c503-5e4c-b319-6b7d917fa37f"),
                Level = "N5",
                Word = "六つ",
                Reading = "むっつ",
                Meaning = "6 cái",
                OrderIndex = 24,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("686d5947-3b29-54dd-8ccc-8bffea484164"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("e7009d91-c503-5e4c-b319-6b7d917fa37f"),
                Level = "N5",
                Word = "六",
                Reading = "ろく",
                Meaning = "Số 6",
                OrderIndex = 25,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("df4dd369-9eab-575f-8933-1a0e4948a486"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("4ac63e13-411a-5b19-a116-c6778cabcc8b"),
                Level = "N5",
                Word = "七日",
                Reading = "なのか",
                Meaning = "Ngày mồng 7",
                OrderIndex = 26,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("fb869af9-e272-5f1f-ad8e-24cbd08c7925"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("4ac63e13-411a-5b19-a116-c6778cabcc8b"),
                Level = "N5",
                Word = "七月",
                Reading = "しちがつ",
                Meaning = "tháng 7",
                OrderIndex = 27,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("d5b34734-ce46-516f-a981-f928b7cc5d34"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("4ac63e13-411a-5b19-a116-c6778cabcc8b"),
                Level = "N5",
                Word = "七つ",
                Reading = "ななつ",
                Meaning = "7 cái",
                OrderIndex = 28,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("8b54890e-ea98-5530-b678-3a939a995d6d"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("4ac63e13-411a-5b19-a116-c6778cabcc8b"),
                Level = "N5",
                Word = "七",
                Reading = "なな/しち",
                Meaning = "Số 7",
                OrderIndex = 29,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("08d25db3-3b95-530e-b475-34531b9e4451"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("ee9d9eb2-e9e3-56d0-8676-47ed4b603367"),
                Level = "N5",
                Word = "八日",
                Reading = "ようか",
                Meaning = "Ngày mồng 8",
                OrderIndex = 30,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("db9066c2-df92-55b9-91a9-c7082b73a6c0"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("ee9d9eb2-e9e3-56d0-8676-47ed4b603367"),
                Level = "N5",
                Word = "八月",
                Reading = "はちがつ",
                Meaning = "Tháng 8",
                OrderIndex = 31,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("af233439-d793-5740-848d-ff33577ef25c"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("ee9d9eb2-e9e3-56d0-8676-47ed4b603367"),
                Level = "N5",
                Word = "八つ",
                Reading = "やっつ",
                Meaning = "8 cái",
                OrderIndex = 32,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("0a135f3c-630a-5ca6-8425-399a85797880"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("ee9d9eb2-e9e3-56d0-8676-47ed4b603367"),
                Level = "N5",
                Word = "八",
                Reading = "はち",
                Meaning = "Số 8",
                OrderIndex = 33,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("99be3c1f-1dee-5545-b4d7-a0fe8ca006fc"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("baf49199-9e6c-5f39-b696-c1ae140ebb7b"),
                Level = "N5",
                Word = "九日",
                Reading = "ここのか",
                Meaning = "Ngày mồng 9",
                OrderIndex = 34,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("b48b8626-2480-5c75-84ee-fb5be0fdca81"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("baf49199-9e6c-5f39-b696-c1ae140ebb7b"),
                Level = "N5",
                Word = "九月",
                Reading = "くがつ",
                Meaning = "tháng 9",
                OrderIndex = 35,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("c841cfac-3db6-5380-98bf-ddbee3da4d8b"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("baf49199-9e6c-5f39-b696-c1ae140ebb7b"),
                Level = "N5",
                Word = "九つ",
                Reading = "ここのつ",
                Meaning = "9 cái",
                OrderIndex = 36,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("2dba3b11-3695-51d9-b2f2-5f427ea3cddb"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("baf49199-9e6c-5f39-b696-c1ae140ebb7b"),
                Level = "N5",
                Word = "九",
                Reading = "きゅう",
                Meaning = "Số 9",
                OrderIndex = 37,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("1fa838a6-2b72-5889-8aac-1b44fb7280ca"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("ddb5d53b-5213-599d-b9be-3536802be0e7"),
                Level = "N5",
                Word = "十日",
                Reading = "とおka",
                Meaning = "Ngày mồng 10",
                OrderIndex = 38,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("4bcf50e7-fd7e-56af-a4bc-941cb1908a84"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("ddb5d53b-5213-599d-b9be-3536802be0e7"),
                Level = "N5",
                Word = "十月",
                Reading = "じゅうがつ",
                Meaning = "tháng 10",
                OrderIndex = 39,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("d57df543-b5f4-517b-b7da-53dbc41ad79a"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("ddb5d53b-5213-599d-b9be-3536802be0e7"),
                Level = "N5",
                Word = "十",
                Reading = "じゅう",
                Meaning = "Số 10",
                OrderIndex = 40,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("05b067a8-f748-50c0-9905-fe48aaecfe98"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("472283d7-6919-56ff-9fec-2d6aa36cc077"),
                Level = "N5",
                Word = "二百",
                Reading = "にひゃく",
                Meaning = "200",
                OrderIndex = 41,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("b51c8a24-0206-5547-bd69-59a27fccca9d"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("08c12d3c-0774-57c9-9fb3-a3ce7b07d4c8"),
                Level = "N5",
                Word = "三百",
                Reading = "さんびゃく",
                Meaning = "300",
                OrderIndex = 42,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("26e61cfb-03a1-5206-a99a-e5e2c53a3509"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("e7009d91-c503-5e4c-b319-6b7d917fa37f"),
                Level = "N5",
                Word = "六百",
                Reading = "ろっぴゃく",
                Meaning = "600",
                OrderIndex = 43,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("2920a7cc-b553-5034-8674-2498c5ae0024"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("ee9d9eb2-e9e3-56d0-8676-47ed4b603367"),
                Level = "N5",
                Word = "八百",
                Reading = "はっぴゃく",
                Meaning = "800",
                OrderIndex = 44,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("99bdce10-eadb-53fe-a6e5-50d3c1049674"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("7e864a3b-ca27-5a05-abd8-a6c16639a2b5"),
                Level = "N5",
                Word = "千",
                Reading = "せん",
                Meaning = "1000",
                OrderIndex = 45,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("c0ee16d2-5312-598d-818e-2c957b13179a"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("08c12d3c-0774-57c9-9fb3-a3ce7b07d4c8"),
                Level = "N5",
                Word = "三千",
                Reading = "さんぜん",
                Meaning = "3000",
                OrderIndex = 46,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("ed3e3a34-1560-5c95-98a8-2df2ca8b6ff7"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("ee9d9eb2-e9e3-56d0-8676-47ed4b603367"),
                Level = "N5",
                Word = "八千",
                Reading = "はっせん",
                Meaning = "8000",
                OrderIndex = 47,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("07a0e297-2b71-5e29-abeb-8ca96cc3f469"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("f67923c2-4634-53f4-8606-fcc50c8de6c4"),
                Level = "N5",
                Word = "一万",
                Reading = "いちまん",
                Meaning = "10.000",
                OrderIndex = 48,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("e284d007-e9dc-5e99-bde7-9083145508cc"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("5b211445-142d-507e-9963-47430a88e47b"),
                Level = "N5",
                Word = "百円",
                Reading = "ひゃくえん",
                Meaning = "100Yên",
                OrderIndex = 49,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("d6ff9178-c5d7-534c-b12f-de1141adbdcc"),
                LessonId = lesson2Id,
                KanjiItemId = Guid.Parse("f67923c2-4634-53f4-8606-fcc50c8de6c4"),
                Level = "N5",
                Word = "一万円",
                Reading = "いちまんえん",
                Meaning = "10.000Yên",
                OrderIndex = 50,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
        };

        foreach (var item in vocabItems2)
        {
            if (!await db.KanjiVocabularyItems.AnyAsync(v => v.Word == item.Word && v.Reading == item.Reading))
                db.KanjiVocabularyItems.Add(item);
        }

        // Lesson 3: Thời gian và Ngày trong tuần (JPD113)
        var lesson3Id = Guid.Parse("d7b94290-67f0-5d2c-bb16-88d3b4d23d7e");
        if (!await db.KanjiLessons.AnyAsync(l => l.Id == lesson3Id))
        {
            db.KanjiLessons.Add(new KanjiLesson { Id = lesson3Id, Level = "N5", LessonNumber = 3, Title = "Thời gian và Ngày trong tuần", Description = "Thời gian và Ngày trong tuần - JPD113 Kanji N5.", AccessTier = "free", PackageCode = "kanji_jpd113", OrderIndex = 3, CreatedAt = SeededAt, UpdatedAt = SeededAt });
        }

        var kanjiItems3 = new List<KanjiItem>
        {
            new KanjiItem {
                Id = Guid.Parse("d01bd13d-d6ae-5471-9ee2-0345fb64c34f"),
                LessonId = lesson3Id,
                Level = "N5",
                Character = "月",
                HanViet = "NGUYỆT",
                Meaning = "Mặt trăng, tháng, thứ Hai",
                StrokeCount = 4,
                KunReading = "つき",
                OnReading = "ゲツ、ガツ",
                Mnemonic = "Hình ảnh mặt trăng (月) khuyết mỏng manh chênh chếch trên bầu trời, có hai đám mây trôi ngang qua ở giữa.",
                ComponentMapJson = @"[]",
                StrokeDataJson = @"[""M34.25,16.25c1,1,1.48,2.38,1.5,4c0.38,33.62,2.38,59.38-11,73.25"", ""M36.25,19c4.12-0.62,31.49-4.78,33.25-5c4-0.5,5.5,1.12,5.5,4.75c0,2.76-0.5,49.25-0.5,69.5c0,13-6.25,4-8.75,1.75"", ""M37.25,38c10.25-1.5,27.25-3.75,36.25-4.5"", ""M37,58.25c8.75-1.12,27-3.5,36.25-4""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_06708"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:06708"" kvg:element=""月"" kvg:radical=""general"">
	<path id=""kvg:06708-s1"" kvg:type=""㇓"" d=""M34.25,16.25c1,1,1.48,2.38,1.5,4c0.38,33.62,2.38,59.38-11,73.25""/>
	<path id=""kvg:06708-s2"" kvg:type=""㇆a"" d=""M36.25,19c4.12-0.62,31.49-4.78,33.25-5c4-0.5,5.5,1.12,5.5,4.75c0,2.76-0.5,49.25-0.5,69.5c0,13-6.25,4-8.75,1.75""/>
	<path id=""kvg:06708-s3"" kvg:type=""㇐a"" d=""M37.25,38c10.25-1.5,27.25-3.75,36.25-4.5""/>
	<path id=""kvg:06708-s4"" kvg:type=""㇐a"" d=""M37,58.25c8.75-1.12,27-3.5,36.25-4""/>
</g>
</g>
<g id=""kvg:StrokeNumbers_06708"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 27.50 23.43)"">1</text>
	<text transform=""matrix(1 0 0 1 37.50 15.50)"">2</text>
	<text transform=""matrix(1 0 0 1 40.00 33.50)"">3</text>
	<text transform=""matrix(1 0 0 1 40.00 54.50)"">4</text>
</g>
</svg>
",
                OrderIndex = 1,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("26528581-0a3a-5beb-a4e1-4bb8802c74fa"),
                LessonId = lesson3Id,
                Level = "N5",
                Character = "火",
                HanViet = "HỎA",
                Meaning = "Lửa, thứ Ba",
                StrokeCount = 4,
                KunReading = "ひ、び、ほ",
                OnReading = "カ",
                Mnemonic = "Người (人) vung hai tay mạnh mẽ tạo ra những tia lửa (火) bùng cháy rực rỡ.",
                ComponentMapJson = @"[{""character"": ""人"", ""component"": ""人"", ""name"": ""人"", ""meaning"": ""人""}]",
                StrokeDataJson = @"[""M24.25,34c3.27,3.33,8.5,13,9.5,17.75"", ""M83,27.25c0.5,1.38,0.22,2.74-0.5,4.25c-2.38,5-7.5,12.12-12.75,17.25"", ""M52.5,14.25c1,1.25,1.5,3.12,1.5,5C54,69,39.62,80,21,91.5"", ""M52.75,50c12.49,14.06,25.01,28.42,33.62,36.13c2.7,2.42,4.9,4.02,8.38,4.87""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_0706b"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:0706b"" kvg:element=""火"" kvg:radical=""general"">
	<path id=""kvg:0706b-s1"" kvg:type=""㇔"" d=""M24.25,34c3.27,3.33,8.5,13,9.5,17.75""/>
	<path id=""kvg:0706b-s2"" kvg:type=""㇒"" d=""M83,27.25c0.5,1.38,0.22,2.74-0.5,4.25c-2.38,5-7.5,12.12-12.75,17.25""/>
	<path id=""kvg:0706b-s3"" kvg:type=""㇒"" d=""M52.5,14.25c1,1.25,1.5,3.12,1.5,5C54,69,39.62,80,21,91.5""/>
	<path id=""kvg:0706b-s4"" kvg:type=""㇏"" d=""M52.75,50c12.49,14.06,25.01,28.42,33.62,36.13c2.7,2.42,4.9,4.02,8.38,4.87""/>
</g>
</g>
<g id=""kvg:StrokeNumbers_0706b"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 16.50 32.50)"">1</text>
	<text transform=""matrix(1 0 0 1 74.50 26.50)"">2</text>
	<text transform=""matrix(1 0 0 1 41.50 14.50)"">3</text>
	<text transform=""matrix(1 0 0 1 55.25 67.13)"">4</text>
</g>
</svg>
",
                OrderIndex = 2,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("bff300d8-ac44-5fef-853d-f2c449f59b34"),
                LessonId = lesson3Id,
                Level = "N5",
                Character = "水",
                HanViet = "THỦY",
                Meaning = "Nước, thứ Tư",
                StrokeCount = 4,
                KunReading = "みず",
                OnReading = "スイ",
                Mnemonic = "Dòng thác chảy thẳng tắp ở giữa (亅) đập vào đá làm nước (水) bắn tung tóe ra hai bên.",
                ComponentMapJson = @"[{""character"": ""亅"", ""component"": ""亅"", ""name"": ""亅"", ""meaning"": ""亅""}]",
                StrokeDataJson = @"[""M52.77,15.08c1.08,1.08,1.67,2.49,1.76,5.52c0.4,14.55-0.26,62.16-0.26,67.12c0,9.78-7.52,0.03-9.02-1.22"", ""M17.5,45.75c1.75,0.62,3.73,0.43,5.25,0C25.88,44.88,36.09,41,38.59,40s4.47,1.24,3.75,3.5C39,54,28.25,69,19,74.75"", ""M81.22,27.5c-0.22,1.25-0.72,2.25-1.52,2.97c-5.64,5.1-12.45,9.78-22.45,13.78"", ""M57,46c8.82,10.73,19.23,21.46,28.42,27.42c2.16,1.4,4.52,3,7.08,3.58""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_06c34"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:06c34"" kvg:element=""水"" kvg:radical=""general"">
	<path id=""kvg:06c34-s1"" kvg:type=""㇚"" d=""M52.77,15.08c1.08,1.08,1.67,2.49,1.76,5.52c0.4,14.55-0.26,62.16-0.26,67.12c0,9.78-7.52,0.03-9.02-1.22""/>
	<path id=""kvg:06c34-s2"" kvg:type=""㇇"" d=""M17.5,45.75c1.75,0.62,3.73,0.43,5.25,0C25.88,44.88,36.09,41,38.59,40s4.47,1.24,3.75,3.5C39,54,28.25,69,19,74.75""/>
	<path id=""kvg:06c34-s3"" kvg:type=""㇒"" d=""M81.22,27.5c-0.22,1.25-0.72,2.25-1.52,2.97c-5.64,5.1-12.45,9.78-22.45,13.78""/>
	<path id=""kvg:06c34-s4"" kvg:type=""㇏"" d=""M57,46c8.82,10.73,19.23,21.46,28.42,27.42c2.16,1.4,4.52,3,7.08,3.58""/>
</g>
</g>
<g id=""kvg:StrokeNumbers_06c34"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 43.75 15.38)"">1</text>
	<text transform=""matrix(1 0 0 1 10.25 47.28)"">2</text>
	<text transform=""matrix(1 0 0 1 83.75 24.28)"">3</text>
	<text transform=""matrix(1 0 0 1 64.00 51.03)"">4</text>
</g>
</svg>
",
                OrderIndex = 3,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("37abdf45-fc81-5dda-8566-2c993fdac3d1"),
                LessonId = lesson3Id,
                Level = "N5",
                Character = "木",
                HanViet = "MỘC",
                Meaning = "Cây, thứ Năm",
                StrokeCount = 4,
                KunReading = "き、こ",
                OnReading = "モク、ボク",
                Mnemonic = "Cành và thân cây dang ra như hình chữ thập (十), bên dưới là rễ cắm sâu xuống đất tạo thành cái cây (木).",
                ComponentMapJson = @"[{""character"": ""十"", ""component"": ""十"", ""name"": ""十"", ""meaning"": ""十""}]",
                StrokeDataJson = @"[""M19.5,39.86c2.45,0.57,5.23,0.8,8.04,0.57C40.75,39.38,63,36.5,79.78,36.15c2.8-0.06,4.54,0.1,7.34,0.5"", ""M51.75,10.5c1.19,1.19,2,3,2,5c0,8.65,0,55.15-0.14,74.75c-0.03,4.19-0.07,7.15-0.11,8.25"", ""M50.75,39.5c0,1.12-0.61,2.44-1.42,3.95C41.75,57.5,26.7,73.93,15.75,80.25"", ""M54.5,39c4.62,6,23,25.75,31.76,34.61c2.27,2.29,4.61,4.39,7.49,5.64""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_06728"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:06728"" kvg:element=""木"" kvg:radical=""general"">
	<path id=""kvg:06728-s1"" kvg:type=""㇐"" d=""M19.5,39.86c2.45,0.57,5.23,0.8,8.04,0.57C40.75,39.38,63,36.5,79.78,36.15c2.8-0.06,4.54,0.1,7.34,0.5""/>
	<path id=""kvg:06728-s2"" kvg:type=""㇑"" d=""M51.75,10.5c1.19,1.19,2,3,2,5c0,8.65,0,55.15-0.14,74.75c-0.03,4.19-0.07,7.15-0.11,8.25""/>
	<path id=""kvg:06728-s3"" kvg:type=""㇒"" d=""M50.75,39.5c0,1.12-0.61,2.44-1.42,3.95C41.75,57.5,26.7,73.93,15.75,80.25""/>
	<path id=""kvg:06728-s4"" kvg:type=""㇏"" d=""M54.5,39c4.62,6,23,25.75,31.76,34.61c2.27,2.29,4.61,4.39,7.49,5.64""/>
</g>
</g>
<g id=""kvg:StrokeNumbers_06728"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 12.50 41.50)"">1</text>
	<text transform=""matrix(1 0 0 1 42.50 11.50)"">2</text>
	<text transform=""matrix(1 0 0 1 37.50 50.50)"">3</text>
	<text transform=""matrix(1 0 0 1 66.50 49.50)"">4</text>
</g>
</svg>
",
                OrderIndex = 4,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("576b6447-5b94-5a7a-957b-aac336483316"),
                LessonId = lesson3Id,
                Level = "N5",
                Character = "金",
                HanViet = "KIM",
                Meaning = "Tiền, vàng, thứ Sáu",
                StrokeCount = 8,
                KunReading = "かね、かな",
                OnReading = "キン、コン",
                Mnemonic = "Được giấu kín dưới mái nhà (𠆢) của nhà vua (王) chính là hai thỏi vàng (ハ) vô giá (金).",
                ComponentMapJson = @"[{""character"": ""王"", ""component"": ""王"", ""name"": ""王"", ""meaning"": ""王""}]",
                StrokeDataJson = @"[""M51.75,11.88c0.25,1.52-0.22,3.57-0.8,4.84C47.73,23.79,33.13,47.1,14.5,58"", ""M52.25,18.25c9.5,7.5,34.14,30.88,37.21,32.67c3.12,1.82,4.14,2.66,5.54,2.83"", ""M34.02,47.08c1.69,0.65,3.85,0.36,5.6,0.21c6.91-0.6,14.33-1.69,23.99-2.64c2.07-0.2,4.1-0.4,6.15,0.12"", ""M30.18,64.96c1.95,0.67,4.47,0.31,6.47,0.12c9.24-0.87,17.42-1.58,31.35-2.53c2.3-0.16,4.68-0.36,6.96,0.08"", ""M51.47,48.82c0.89,0.85,0.89,3.76,0.89,4.43c0,3.64,0.27,38.71,0.22,39.82"", ""M31,74.75c3.25,3,7.48,9.27,8.5,12"", ""M73.01,72.11c0.24,1.14,0.11,2.46-0.54,3.51C70.38,79,66.44,83.22,63,86"", ""M18.5,94.86c2.88,1.01,6.41,0.4,9.37,0.15c16.55-1.42,32.95-2.12,51.51-3c3.13-0.15,6.32-0.27,9.38,0.59""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_091d1"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:091d1"" kvg:element=""金"" kvg:radical=""general"">
	<g id=""kvg:091d1-g1"" kvg:position=""top"" kvg:phon=""今1"">
		<path id=""kvg:091d1-s1"" kvg:type=""㇒"" d=""M51.75,11.88c0.25,1.52-0.22,3.57-0.8,4.84C47.73,23.79,33.13,47.1,14.5,58""/>
		<path id=""kvg:091d1-s2"" kvg:type=""㇏"" d=""M52.25,18.25c9.5,7.5,34.14,30.88,37.21,32.67c3.12,1.82,4.14,2.66,5.54,2.83""/>
	</g>
	<g id=""kvg:091d1-g2"" kvg:position=""bottom"">
		<g id=""kvg:091d1-g3"" kvg:phon=""今2"">
			<path id=""kvg:091d1-s3"" kvg:type=""㇐"" d=""M34.02,47.08c1.69,0.65,3.85,0.36,5.6,0.21c6.91-0.6,14.33-1.69,23.99-2.64c2.07-0.2,4.1-0.4,6.15,0.12""/>
			<path id=""kvg:091d1-s4"" kvg:type=""㇐"" d=""M30.18,64.96c1.95,0.67,4.47,0.31,6.47,0.12c9.24-0.87,17.42-1.58,31.35-2.53c2.3-0.16,4.68-0.36,6.96,0.08""/>
		</g>
		<path id=""kvg:091d1-s5"" kvg:type=""㇑a"" d=""M51.47,48.82c0.89,0.85,0.89,3.76,0.89,4.43c0,3.64,0.27,38.71,0.22,39.82""/>
		<path id=""kvg:091d1-s6"" kvg:type=""㇔"" d=""M31,74.75c3.25,3,7.48,9.27,8.5,12""/>
		<path id=""kvg:091d1-s7"" kvg:type=""㇒"" d=""M73.01,72.11c0.24,1.14,0.11,2.46-0.54,3.51C70.38,79,66.44,83.22,63,86""/>
		<path id=""kvg:091d1-s8"" kvg:type=""㇐"" d=""M18.5,94.86c2.88,1.01,6.41,0.4,9.37,0.15c16.55-1.42,32.95-2.12,51.51-3c3.13-0.15,6.32-0.27,9.38,0.59""/>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_091d1"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 43.25 13.13)"">1</text>
	<text transform=""matrix(1 0 0 1 60.50 21.50)"">2</text>
	<text transform=""matrix(1 0 0 1 42.50 43.50)"">3</text>
	<text transform=""matrix(1 0 0 1 21.50 66.50)"">4</text>
	<text transform=""matrix(1 0 0 1 56.50 54.50)"">5</text>
	<text transform=""matrix(1 0 0 1 23.50 78.50)"">6</text>
	<text transform=""matrix(1 0 0 1 78.50 74.50)"">7</text>
	<text transform=""matrix(1 0 0 1 10.50 96.50)"">8</text>
</g>
</svg>
",
                OrderIndex = 5,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("118222bb-2f95-564f-8ff5-beb7c16e97e8"),
                LessonId = lesson3Id,
                Level = "N5",
                Character = "土",
                HanViet = "THỔ",
                Meaning = "Đất, thứ Bảy",
                StrokeCount = 3,
                KunReading = "つち",
                OnReading = "ド、ト",
                Mnemonic = "Cắm một cây thập tự giá (十) sâu xuống mặt đất (一) bằng phẳng (土).",
                ComponentMapJson = @"[{""character"": ""十"", ""component"": ""十"", ""name"": ""十"", ""meaning"": ""十""}, {""character"": ""一"", ""component"": ""一"", ""name"": ""一"", ""meaning"": ""一""}]",
                StrokeDataJson = @"[""M26.63,50.89c1.63,0.4,4.64,0.6,6.26,0.4C43.5,50,62.12,48,75.66,46.92c2.71-0.22,4.36,0.19,5.72,0.39"", ""M52.17,17.37c1.17,1.17,2.02,3.13,2.02,4.64c0,10.25,0.14,61.06,0.14,63.36"", ""M15.38,87.73c2.12,0.54,6.01,0.73,8.12,0.54C46,86.25,69,84.62,90.34,83.79c3.53-0.14,5.65,0.26,7.41,0.53""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_0571f"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:0571f"" kvg:element=""土"" kvg:radical=""general"">
	<path id=""kvg:0571f-s1"" kvg:type=""㇐"" d=""M26.63,50.89c1.63,0.4,4.64,0.6,6.26,0.4C43.5,50,62.12,48,75.66,46.92c2.71-0.22,4.36,0.19,5.72,0.39""/>
	<path id=""kvg:0571f-s2"" kvg:type=""㇑a"" d=""M52.17,17.37c1.17,1.17,2.02,3.13,2.02,4.64c0,10.25,0.14,61.06,0.14,63.36""/>
	<path id=""kvg:0571f-s3"" kvg:type=""㇐"" d=""M15.38,87.73c2.12,0.54,6.01,0.73,8.12,0.54C46,86.25,69,84.62,90.34,83.79c3.53-0.14,5.65,0.26,7.41,0.53""/>
</g>
</g>
<g id=""kvg:StrokeNumbers_0571f"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 19.50 51.13)"">1</text>
	<text transform=""matrix(1 0 0 1 41.50 17.50)"">2</text>
	<text transform=""matrix(1 0 0 1 6.50 86.50)"">3</text>
</g>
</svg>
",
                OrderIndex = 6,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("54a45487-e51f-53bd-9c70-87529905cd36"),
                LessonId = lesson3Id,
                Level = "N5",
                Character = "何",
                HanViet = "HÀ",
                Meaning = "Cái gì",
                StrokeCount = 7,
                KunReading = "なに、なん",
                OnReading = "カ",
                Mnemonic = "Một người (亻) luôn tò mò hỏi xem bản thân có thể (可) làm được cái gì (何).",
                ComponentMapJson = @"[{""character"": ""亻"", ""component"": ""亻"", ""name"": ""亻"", ""meaning"": ""亻""}, {""character"": ""可"", ""component"": ""可"", ""name"": ""可"", ""meaning"": ""可""}]",
                StrokeDataJson = @"[""M32.5,13.75c0.23,2.1-0.19,3.81-0.8,5.66c-3.95,11.84-9.67,23.37-20.45,37.34"", ""M26.76,36.5c1.24,1.5,1.54,3.04,1.54,5.5c0,9.46-0.13,30.79-0.17,44.62c-0.01,2.6-0.01,4.94-0.01,6.88"", ""M38.88,26.64c1.74,0.5,4.68,0.67,6.41,0.5c13.21-1.27,33.84-4.77,46.26-5.86c2.88-0.25,4.63,0.24,6.08,0.49"", ""M40.87,44c0.75,0.75,1.26,1.62,1.36,2.21c0.67,4.06,1.44,10.16,2.25,16.3c0.27,2.04,0.26,2.01,0.47,3.75"", ""M43.27,45.6c6.28-1.17,15.14-2.97,19.73-3.72c3.13-0.51,4.4,0.31,3.68,3.51c-0.86,3.86-2.49,10.33-3.28,14.14"", ""M45.59,63.17c3.71-0.39,9.45-1.24,14.45-1.85c1.89-0.23,3.82-0.47,5.75-0.69"", ""M83.25,24.75c1,1,1.74,2.18,1.81,4.99c0.33,13.52-0.21,56.44-0.21,61.04c0,10.71-6.35,2.71-9.18-0.77""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_04f55"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:04f55"" kvg:element=""何"">
	<g id=""kvg:04f55-g1"" kvg:element=""亻"" kvg:original=""人"" kvg:position=""left"" kvg:radical=""general"">
		<path id=""kvg:04f55-s1"" kvg:type=""㇒"" d=""M32.5,13.75c0.23,2.1-0.19,3.81-0.8,5.66c-3.95,11.84-9.67,23.37-20.45,37.34""/>
		<path id=""kvg:04f55-s2"" kvg:type=""㇑"" d=""M26.76,36.5c1.24,1.5,1.54,3.04,1.54,5.5c0,9.46-0.13,30.79-0.17,44.62c-0.01,2.6-0.01,4.94-0.01,6.88""/>
	</g>
	<g id=""kvg:04f55-g2"" kvg:element=""可"" kvg:position=""right"" kvg:phon=""可"">
		<g id=""kvg:04f55-g3"" kvg:element=""丁"" kvg:part=""1"">
			<g id=""kvg:04f55-g4"" kvg:element=""一"">
				<path id=""kvg:04f55-s3"" kvg:type=""㇐"" d=""M38.88,26.64c1.74,0.5,4.68,0.67,6.41,0.5c13.21-1.27,33.84-4.77,46.26-5.86c2.88-0.25,4.63,0.24,6.08,0.49""/>
			</g>
		</g>
		<g id=""kvg:04f55-g5"" kvg:element=""口"">
			<path id=""kvg:04f55-s4"" kvg:type=""㇑"" d=""M40.87,44c0.75,0.75,1.26,1.62,1.36,2.21c0.67,4.06,1.44,10.16,2.25,16.3c0.27,2.04,0.26,2.01,0.47,3.75""/>
			<path id=""kvg:04f55-s5"" kvg:type=""㇕b"" d=""M43.27,45.6c6.28-1.17,15.14-2.97,19.73-3.72c3.13-0.51,4.4,0.31,3.68,3.51c-0.86,3.86-2.49,10.33-3.28,14.14""/>
			<path id=""kvg:04f55-s6"" kvg:type=""㇐b"" d=""M45.59,63.17c3.71-0.39,9.45-1.24,14.45-1.85c1.89-0.23,3.82-0.47,5.75-0.69""/>
		</g>
		<g id=""kvg:04f55-g6"" kvg:element=""丁"" kvg:part=""2"">
			<g id=""kvg:04f55-g7"" kvg:element=""亅"">
				<path id=""kvg:04f55-s7"" kvg:type=""㇚"" d=""M83.25,24.75c1,1,1.74,2.18,1.81,4.99c0.33,13.52-0.21,56.44-0.21,61.04c0,10.71-6.35,2.71-9.18-0.77""/>
			</g>
		</g>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_04f55"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 24.50 13.50)"">1</text>
	<text transform=""matrix(1 0 0 1 20.50 55.50)"">2</text>
	<text transform=""matrix(1 0 0 1 42.75 24.50)"">3</text>
	<text transform=""matrix(1 0 0 1 35.50 52.63)"">4</text>
	<text transform=""matrix(1 0 0 1 44.25 42.13)"">5</text>
	<text transform=""matrix(1 0 0 1 48.75 59.50)"">6</text>
	<text transform=""matrix(1 0 0 1 76.50 34.63)"">7</text>
</g>
</svg>
",
                OrderIndex = 7,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("2473b071-dd7a-561b-9b5a-67dabe649765"),
                LessonId = lesson3Id,
                Level = "N5",
                Character = "年",
                HanViet = "NIÊN",
                Meaning = "Năm",
                StrokeCount = 6,
                KunReading = "とし",
                OnReading = "ネン",
                Mnemonic = "Người (𠂉) nông dân làm việc vất vả thu hoạch được một (一) chục (十) vụ mùa là trôi qua hết một năm (年).",
                ComponentMapJson = @"[{""character"": ""一"", ""component"": ""一"", ""name"": ""一"", ""meaning"": ""一""}, {""character"": ""十"", ""component"": ""十"", ""name"": ""十"", ""meaning"": ""十""}]",
                StrokeDataJson = @"[""M40.01,11.89c0.24,1.61-0.01,2.86-0.84,4.46c-2.53,4.84-6.91,11.4-15.86,19.62"", ""M39.13,23.62c2.25,0.38,4.4,0.18,5.79,0.03c11.7-1.27,21.33-2.9,33.22-4.07c2.3-0.23,4.2,0,5.35,0.26"", ""M30.13,43.59c1.36,0.33,3.87,0.46,5.21,0.33c10.91-1.05,28.53-3.42,40.78-4.26c2.26-0.15,3.63,0.16,4.76,0.32"", ""M33.75,44.5c1,1.25,1,1.97,1.01,3.5C34.8,52.33,35,65.29,35,66.25"", ""M13.88,67.74c1.97,0.47,5.61,0.66,7.57,0.47c20.21-2.03,36.35-4.62,66.65-5.31c3.29-0.08,5.26,0.22,6.91,0.46"", ""M56.56,25.46c1.12,1.12,1.79,3.54,1.79,4.94c0,0.89-0.05,44.26-0.13,61.6c-0.01,3.12-0.03,5.39-0.05,6.38""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_05e74"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:05e74"" kvg:element=""年"">
	<g id=""kvg:05e74-g1"" kvg:element=""丿"" kvg:radical=""nelson"">
		<path id=""kvg:05e74-s1"" kvg:type=""㇒"" d=""M40.01,11.89c0.24,1.61-0.01,2.86-0.84,4.46c-2.53,4.84-6.91,11.4-15.86,19.62""/>
	</g>
	<g id=""kvg:05e74-g2"" kvg:element=""干"" kvg:part=""1"" kvg:radical=""tradit"">
		<path id=""kvg:05e74-s2"" kvg:type=""㇐"" d=""M39.13,23.62c2.25,0.38,4.4,0.18,5.79,0.03c11.7-1.27,21.33-2.9,33.22-4.07c2.3-0.23,4.2,0,5.35,0.26""/>
		<g id=""kvg:05e74-g3"" kvg:element=""十"">
			<path id=""kvg:05e74-s3"" kvg:type=""㇐"" d=""M30.13,43.59c1.36,0.33,3.87,0.46,5.21,0.33c10.91-1.05,28.53-3.42,40.78-4.26c2.26-0.15,3.63,0.16,4.76,0.32""/>
		</g>
	</g>
	<path id=""kvg:05e74-s4"" kvg:type=""㇑a"" d=""M33.75,44.5c1,1.25,1,1.97,1.01,3.5C34.8,52.33,35,65.29,35,66.25""/>
	<path id=""kvg:05e74-s5"" kvg:type=""㇐"" d=""M13.88,67.74c1.97,0.47,5.61,0.66,7.57,0.47c20.21-2.03,36.35-4.62,66.65-5.31c3.29-0.08,5.26,0.22,6.91,0.46""/>
	<g id=""kvg:05e74-g4"" kvg:element=""干"" kvg:part=""2"" kvg:radical=""tradit"">
		<path id=""kvg:05e74-s6"" kvg:type=""㇑"" d=""M56.56,25.46c1.12,1.12,1.79,3.54,1.79,4.94c0,0.89-0.05,44.26-0.13,61.6c-0.01,3.12-0.03,5.39-0.05,6.38""/>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_05e74"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 32.25 11.50)"">1</text>
	<text transform=""matrix(1 0 0 1 46.50 19.50)"">2</text>
	<text transform=""matrix(1 0 0 1 31.50 41.50)"">3</text>
	<text transform=""matrix(1 0 0 1 27.50 53.50)"">4</text>
	<text transform=""matrix(1 0 0 1 6.50 69.50)"">5</text>
	<text transform=""matrix(1 0 0 1 50.50 33.50)"">6</text>
</g>
</svg>
",
                OrderIndex = 8,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("eff056fa-5214-5e6d-b2a3-c677dc8655fc"),
                LessonId = lesson3Id,
                Level = "N5",
                Character = "時",
                HanViet = "THỜI",
                Meaning = "Thời gian, giờ",
                StrokeCount = 10,
                KunReading = "とき",
                OnReading = "ジ",
                Mnemonic = "Ngày xưa người ta nhìn mặt trời (日) mọc trên đỉnh nóc chùa (寺) để biết thời gian (時) và giờ giấc.",
                ComponentMapJson = @"[{""character"": ""日"", ""component"": ""日"", ""name"": ""日"", ""meaning"": ""日""}, {""character"": ""寺"", ""component"": ""寺"", ""name"": ""寺"", ""meaning"": ""寺""}]",
                StrokeDataJson = @"[""M16,29.84c0.75,0.66,1.21,1.62,1.21,3.07c0,1.18-0.16,30.08-0.21,40.85c-0.01,2.42-0.02,3.95-0.02,4.08"", ""M17.78,30.74c4.65-0.63,16.12-2.07,17.6-2.25c1.52-0.18,3,1.5,2.88,2.57c-0.24,2.17-0.36,24.9-0.35,40.79c0,1.63-0.12,3.35-0.12,4.43"", ""M18.75,52c4.5-0.75,13.5-2.12,18.22-2.35"", ""M17.8,74.52c6.2-0.92,11.45-1.89,18.94-2.7"", ""M51.44,29.03c1.37,0.44,3.63,0.34,5,0.19c9.79-1.09,16.34-2.34,25.62-3.08c2.27-0.18,3.9-0.04,5.04,0.18"", ""M67.59,11.37c0.89,0.9,1.59,2.24,1.59,3.75c0,8.39,0.03,27.02,0.03,27.6"", ""M45.38,45.35c1.49,0.44,4.21,0.59,5.71,0.44c11.29-1.16,25.66-3.29,39.2-3.99c2.48-0.13,3.97,0.21,5.21,0.43"", ""M46,60.99c1.43,0.46,4.04,0.58,5.49,0.46c12.01-1.07,26.89-3.07,39.07-3.89c2.38-0.16,4.41,0.22,5.6,0.44"", ""M78.07,46.08c1.11,1.11,1.66,2.56,1.71,5.06c0.23,12.03-0.09,34.43-0.09,38.52c0,9.83-5.42,2.19-7.66-0.04"", ""M56.75,70.38c2.87,1.76,6.55,6.38,7.27,9.12""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_06642"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:06642"" kvg:element=""時"">
	<g id=""kvg:06642-g1"" kvg:element=""日"" kvg:position=""left"" kvg:radical=""general"">
		<path id=""kvg:06642-s1"" kvg:type=""㇑"" d=""M16,29.84c0.75,0.66,1.21,1.62,1.21,3.07c0,1.18-0.16,30.08-0.21,40.85c-0.01,2.42-0.02,3.95-0.02,4.08""/>
		<path id=""kvg:06642-s2"" kvg:type=""㇕a"" d=""M17.78,30.74c4.65-0.63,16.12-2.07,17.6-2.25c1.52-0.18,3,1.5,2.88,2.57c-0.24,2.17-0.36,24.9-0.35,40.79c0,1.63-0.12,3.35-0.12,4.43""/>
		<path id=""kvg:06642-s3"" kvg:type=""㇐a"" d=""M18.75,52c4.5-0.75,13.5-2.12,18.22-2.35""/>
		<path id=""kvg:06642-s4"" kvg:type=""㇐a"" d=""M17.8,74.52c6.2-0.92,11.45-1.89,18.94-2.7""/>
	</g>
	<g id=""kvg:06642-g2"" kvg:element=""寺"" kvg:position=""right"" kvg:phon=""寺"">
		<g id=""kvg:06642-g3"" kvg:element=""土"">
			<path id=""kvg:06642-s5"" kvg:type=""㇐"" d=""M51.44,29.03c1.37,0.44,3.63,0.34,5,0.19c9.79-1.09,16.34-2.34,25.62-3.08c2.27-0.18,3.9-0.04,5.04,0.18""/>
			<path id=""kvg:06642-s6"" kvg:type=""㇑a"" d=""M67.59,11.37c0.89,0.9,1.59,2.24,1.59,3.75c0,8.39,0.03,27.02,0.03,27.6""/>
			<path id=""kvg:06642-s7"" kvg:type=""㇐"" d=""M45.38,45.35c1.49,0.44,4.21,0.59,5.71,0.44c11.29-1.16,25.66-3.29,39.2-3.99c2.48-0.13,3.97,0.21,5.21,0.43""/>
		</g>
		<g id=""kvg:06642-g4"" kvg:element=""寸"">
			<path id=""kvg:06642-s8"" kvg:type=""㇐"" d=""M46,60.99c1.43,0.46,4.04,0.58,5.49,0.46c12.01-1.07,26.89-3.07,39.07-3.89c2.38-0.16,4.41,0.22,5.6,0.44""/>
			<path id=""kvg:06642-s9"" kvg:type=""㇚"" d=""M78.07,46.08c1.11,1.11,1.66,2.56,1.71,5.06c0.23,12.03-0.09,34.43-0.09,38.52c0,9.83-5.42,2.19-7.66-0.04""/>
			<path id=""kvg:06642-s10"" kvg:type=""㇔"" d=""M56.75,70.38c2.87,1.76,6.55,6.38,7.27,9.12""/>
		</g>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_06642"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 9.75 39.13)"">1</text>
	<text transform=""matrix(1 0 0 1 18.50 27.50)"">2</text>
	<text transform=""matrix(1 0 0 1 21.50 48.13)"">3</text>
	<text transform=""matrix(1 0 0 1 21.33 70.50)"">4</text>
	<text transform=""matrix(1 0 0 1 45.50 27.13)"">5</text>
	<text transform=""matrix(1 0 0 1 58.50 11.50)"">6</text>
	<text transform=""matrix(1 0 0 1 45.75 42.13)"">7</text>
	<text transform=""matrix(1 0 0 1 44.25 57.13)"">8</text>
	<text transform=""matrix(1 0 0 1 71.25 54.13)"">9</text>
	<text transform=""matrix(1 0 0 1 47.25 76.63)"">10</text>
</g>
</svg>
",
                OrderIndex = 9,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("53df3cf0-0b28-5b80-bb4d-07a39190aabd"),
                LessonId = lesson3Id,
                Level = "N5",
                Character = "間",
                HanViet = "GIAN",
                Meaning = "Khoảng trống, ở giữa, trong~, gian phòng",
                StrokeCount = 12,
                KunReading = "あいだ、ま",
                OnReading = "カン、ケン",
                Mnemonic = "Ánh mặt trời (日) len lỏi chiếu qua khe hở ở giữa hai cánh cổng (門) lớn tạo ra một không gian (間) rực sáng.",
                ComponentMapJson = @"[{""character"": ""日"", ""component"": ""日"", ""name"": ""日"", ""meaning"": ""日""}, {""character"": ""門"", ""component"": ""門"", ""name"": ""門"", ""meaning"": ""門""}]",
                StrokeDataJson = @"[""M18.64,15.3c0.71,0.71,1.18,1.82,1.18,3.43c0,3.89-0.05,56.65-0.19,72.77c-0.02,1.92-0.03,4.03-0.05,4.65"", ""M21.01,16.81c5.75-0.6,18.73-2.74,20.5-2.84c1.85-0.1,2.86,0.28,2.9,2.02c0.06,2.75-0.5,16.1-0.85,20.76c-0.12,1.55-0.19,2.57-0.19,2.7"", ""M20.95,27.27c5.99-0.61,14.92-2.02,21.88-2.6"", ""M21.02,39.04c8.11-1.19,14.14-2.1,21.31-2.64"", ""M63.19,13.1c0.98,0.98,1.34,2.15,1.34,2.97c0,5.8-0.08,12.65-0.06,18.93c0.01,2.01,0.02,3.4,0.06,3.58"", ""M65.32,14.77c5.97-0.68,20.69-3.19,22.38-3.28c1.8-0.09,2.81,0.88,2.81,2.82c0,17-0.22,66.12-0.22,78.44c0,10.5-6.35,1.36-7.72,0.23"", ""M65.63,24.79c4.49-0.42,19.73-1.99,23.35-1.99"", ""M65.22,36.07c6.41-0.32,16.53-1.32,23.49-1.81"", ""M40.56,50.95c0.74,0.74,1.04,1.93,1.04,2.99c0,0.83-0.08,20.84-0.05,29.06c0.01,2.25,0.02,2.77,0.05,3"", ""M42.26,52.09c5.56-0.48,19.71-1.98,21.3-2.1c1.68-0.13,2.76,1.46,2.63,2.24c-0.21,1.24-0.41,20.66-0.48,29.02c-0.02,2.29-0.03,3.8-0.03,3.97"", ""M42.64,66.6c5.11-0.48,17.36-1.73,21.88-2.07"", ""M42.25,82.8c5.13-0.3,17-1.55,22.26-2.05""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_09593"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:09593"" kvg:element=""間"">
	<g id=""kvg:09593-g1"" kvg:element=""門"" kvg:position=""kamae"" kvg:radical=""general"">
		<g id=""kvg:09593-g2"" kvg:position=""left"">
			<path id=""kvg:09593-s1"" kvg:type=""㇑"" d=""M18.64,15.3c0.71,0.71,1.18,1.82,1.18,3.43c0,3.89-0.05,56.65-0.19,72.77c-0.02,1.92-0.03,4.03-0.05,4.65""/>
			<path id=""kvg:09593-s2"" kvg:type=""㇕a"" d=""M21.01,16.81c5.75-0.6,18.73-2.74,20.5-2.84c1.85-0.1,2.86,0.28,2.9,2.02c0.06,2.75-0.5,16.1-0.85,20.76c-0.12,1.55-0.19,2.57-0.19,2.7""/>
			<path id=""kvg:09593-s3"" kvg:type=""㇐a"" d=""M20.95,27.27c5.99-0.61,14.92-2.02,21.88-2.6""/>
			<path id=""kvg:09593-s4"" kvg:type=""㇐a"" d=""M21.02,39.04c8.11-1.19,14.14-2.1,21.31-2.64""/>
		</g>
		<g id=""kvg:09593-g3"" kvg:position=""right"">
			<path id=""kvg:09593-s5"" kvg:type=""㇑"" d=""M63.19,13.1c0.98,0.98,1.34,2.15,1.34,2.97c0,5.8-0.08,12.65-0.06,18.93c0.01,2.01,0.02,3.4,0.06,3.58""/>
			<path id=""kvg:09593-s6"" kvg:type=""㇆a"" d=""M65.32,14.77c5.97-0.68,20.69-3.19,22.38-3.28c1.8-0.09,2.81,0.88,2.81,2.82c0,17-0.22,66.12-0.22,78.44c0,10.5-6.35,1.36-7.72,0.23""/>
			<path id=""kvg:09593-s7"" kvg:type=""㇐a"" d=""M65.63,24.79c4.49-0.42,19.73-1.99,23.35-1.99""/>
			<path id=""kvg:09593-s8"" kvg:type=""㇐a"" d=""M65.22,36.07c6.41-0.32,16.53-1.32,23.49-1.81""/>
		</g>
	</g>
	<g id=""kvg:09593-g4"" kvg:element=""日"">
		<path id=""kvg:09593-s9"" kvg:type=""㇑"" d=""M40.56,50.95c0.74,0.74,1.04,1.93,1.04,2.99c0,0.83-0.08,20.84-0.05,29.06c0.01,2.25,0.02,2.77,0.05,3""/>
		<path id=""kvg:09593-s10"" kvg:type=""㇕a"" d=""M42.26,52.09c5.56-0.48,19.71-1.98,21.3-2.1c1.68-0.13,2.76,1.46,2.63,2.24c-0.21,1.24-0.41,20.66-0.48,29.02c-0.02,2.29-0.03,3.8-0.03,3.97""/>
		<path id=""kvg:09593-s11"" kvg:type=""㇐a"" d=""M42.64,66.6c5.11-0.48,17.36-1.73,21.88-2.07""/>
		<path id=""kvg:09593-s12"" kvg:type=""㇐a"" d=""M42.25,82.8c5.13-0.3,17-1.55,22.26-2.05""/>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_09593"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 12.75 23.50)"">1</text>
	<text transform=""matrix(1 0 0 1 21.75 13.63)"">2</text>
	<text transform=""matrix(1 0 0 1 24.75 24.50)"">3</text>
	<text transform=""matrix(1 0 0 1 24.75 35.50)"">4</text>
	<text transform=""matrix(1 0 0 1 56.50 21.50)"">5</text>
	<text transform=""matrix(1 0 0 1 65.25 11.50)"">6</text>
	<text transform=""matrix(1 0 0 1 68.54 22.50)"">7</text>
	<text transform=""matrix(1 0 0 1 68.50 33.50)"">8</text>
	<text transform=""matrix(1 0 0 1 34.50 57.50)"">9</text>
	<text transform=""matrix(1 0 0 1 42.75 48.50)"">10</text>
	<text transform=""matrix(1 0 0 1 44.50 63.50)"">11</text>
	<text transform=""matrix(1 0 0 1 44.31 79.50)"">12</text>
</g>
</svg>
",
                OrderIndex = 10,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("77624de6-8922-5e86-aa9a-824acd663ed6"),
                LessonId = lesson3Id,
                Level = "N5",
                Character = "分",
                HanViet = "PHÂN",
                Meaning = "Phân chia, hiểu, phút",
                StrokeCount = 4,
                KunReading = "わ.ける、わ.かる",
                OnReading = "ブン、フン、ブ",
                Mnemonic = "Dùng con dao (刀) sắc bén để chia đồ vật ra làm tám (ハ) phần (分).",
                ComponentMapJson = @"[{""character"": ""刀"", ""component"": ""刀"", ""name"": ""刀"", ""meaning"": ""刀""}]",
                StrokeDataJson = @"[""M41.12,19.38c0.25,1.24-0.44,3.01-1.1,4.08C34.31,32.72,27.13,40.62,13,51.25"", ""M54.69,13.75c7.56-0.12,20.68,19.17,29.41,25.95c3.07,2.39,6.02,4.05,10.4,5.3"", ""M29.35,55.37c2.42,0.83,4.97,0.75,7.42,0.37c11.06-1.7,28.87-5.3,34.1-5.76c3.69-0.33,5.08,1.48,4.88,3.77c-0.54,6.05-5.94,29.03-10.5,36.3c-4.99,7.96-5.74,4.21-9.84-0.49"", ""M49.12,57.25c0.15,1.49,0.06,2.72-0.56,4.08C43.5,72.5,35.75,82,22.5,90.75""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_05206"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:05206"" kvg:element=""分"">
	<g id=""kvg:05206-g1"" kvg:element=""八"" kvg:position=""top"" kvg:radical=""nelson"">
		<path id=""kvg:05206-s1"" kvg:type=""㇒"" d=""M41.12,19.38c0.25,1.24-0.44,3.01-1.1,4.08C34.31,32.72,27.13,40.62,13,51.25""/>
		<path id=""kvg:05206-s2"" kvg:type=""㇏"" d=""M54.69,13.75c7.56-0.12,20.68,19.17,29.41,25.95c3.07,2.39,6.02,4.05,10.4,5.3""/>
	</g>
	<g id=""kvg:05206-g2"" kvg:element=""刀"" kvg:position=""bottom"" kvg:radical=""tradit"">
		<path id=""kvg:05206-s3"" kvg:type=""㇆"" d=""M29.35,55.37c2.42,0.83,4.97,0.75,7.42,0.37c11.06-1.7,28.87-5.3,34.1-5.76c3.69-0.33,5.08,1.48,4.88,3.77c-0.54,6.05-5.94,29.03-10.5,36.3c-4.99,7.96-5.74,4.21-9.84-0.49""/>
		<path id=""kvg:05206-s4"" kvg:type=""㇒"" d=""M49.12,57.25c0.15,1.49,0.06,2.72-0.56,4.08C43.5,72.5,35.75,82,22.5,90.75""/>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_05206"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 32.50 19.50)"">1</text>
	<text transform=""matrix(1 0 0 1 50.50 11.50)"">2</text>
	<text transform=""matrix(1 0 0 1 30.50 52.50)"">3</text>
	<text transform=""matrix(1 0 0 1 39.50 65.50)"">4</text>
</g>
</svg>
",
                OrderIndex = 11,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
        };

        foreach (var item in kanjiItems3)
        {
            if (!await db.KanjiItems.AnyAsync(k => k.Character == item.Character))
                db.KanjiItems.Add(item);
        }

        var vocabItems3 = new List<KanjiVocabulary>
        {
            new KanjiVocabulary {
                Id = Guid.Parse("f1c4fbcb-1919-5386-9d15-05c2785af3dc"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("d01bd13d-d6ae-5471-9ee2-0345fb64c34f"),
                Level = "N5",
                Word = "月",
                Reading = "つき",
                Meaning = "mặt trăng",
                OrderIndex = 1,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("938d1a34-229f-59cd-aa2c-2623e36dd065"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("d01bd13d-d6ae-5471-9ee2-0345fb64c34f"),
                Level = "N5",
                Word = "一月",
                Reading = "いちがつ",
                Meaning = "Tháng 1",
                OrderIndex = 2,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("2416a85b-540f-5999-ad05-529dd1d327f4"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("d01bd13d-d6ae-5471-9ee2-0345fb64c34f"),
                Level = "N5",
                Word = "十二月",
                Reading = "じゅうにがつ",
                Meaning = "Tháng 12",
                OrderIndex = 3,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("8b80eb00-4895-5b75-9ad8-b3be350a9086"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("d01bd13d-d6ae-5471-9ee2-0345fb64c34f"),
                Level = "N5",
                Word = "月曜日",
                Reading = "げつようび",
                Meaning = "Thứ 2",
                OrderIndex = 4,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("2f80fec5-4a5c-51bb-ac79-b157c26f32fc"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("26528581-0a3a-5beb-a4e1-4bb8802c74fa"),
                Level = "N5",
                Word = "花火",
                Reading = "はなび",
                Meaning = "pháo hoa",
                OrderIndex = 5,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("dd40b42e-6683-567a-98f3-398ba71b98d8"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("26528581-0a3a-5beb-a4e1-4bb8802c74fa"),
                Level = "N5",
                Word = "火",
                Reading = "ひ",
                Meaning = "lửa",
                OrderIndex = 6,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("79189154-b75e-5ea7-8fd4-be73bade0c19"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("26528581-0a3a-5beb-a4e1-4bb8802c74fa"),
                Level = "N5",
                Word = "火曜日",
                Reading = "かようび",
                Meaning = "Thứ 3",
                OrderIndex = 7,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("35efb563-efb7-5520-9f71-218e78c3b1f7"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("bff300d8-ac44-5fef-853d-f2c449f59b34"),
                Level = "N5",
                Word = "水",
                Reading = "みず",
                Meaning = "nước",
                OrderIndex = 8,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("2bbbb796-31f8-5ba2-b7f8-b711c2c547c4"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("bff300d8-ac44-5fef-853d-f2c449f59b34"),
                Level = "N5",
                Word = "水曜日",
                Reading = "すいようび",
                Meaning = "Thứ 4",
                OrderIndex = 9,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("49ebfa10-9656-515f-8fdc-23edb27c4e16"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("37abdf45-fc81-5dda-8566-2c993fdac3d1"),
                Level = "N5",
                Word = "木",
                Reading = "き",
                Meaning = "Cây",
                OrderIndex = 10,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("9be0f3b0-3256-5926-afb6-57f1658916ea"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("37abdf45-fc81-5dda-8566-2c993fdac3d1"),
                Level = "N5",
                Word = "木曜日",
                Reading = "もくようび",
                Meaning = "Thứ 5",
                OrderIndex = 11,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("4c71c876-00b8-5b3c-bec6-7793137d67db"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("576b6447-5b94-5a7a-957b-aac336483316"),
                Level = "N5",
                Word = "お金",
                Reading = "おかね",
                Meaning = "tiền",
                OrderIndex = 12,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("c00e9ddb-4523-58d1-b69c-df48006c4add"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("576b6447-5b94-5a7a-957b-aac336483316"),
                Level = "N5",
                Word = "金曜日",
                Reading = "きんようび",
                Meaning = "Thứ 6",
                OrderIndex = 13,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("af495d7a-3a86-5709-85a2-f676672fee79"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("118222bb-2f95-564f-8ff5-beb7c16e97e8"),
                Level = "N5",
                Word = "土",
                Reading = "つち",
                Meaning = "Đất",
                OrderIndex = 14,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("dc7b6369-9c06-5ac8-ad81-e5def41c340a"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("118222bb-2f95-564f-8ff5-beb7c16e97e8"),
                Level = "N5",
                Word = "土曜日",
                Reading = "どようび",
                Meaning = "Thứ 7",
                OrderIndex = 15,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("8a0abdd5-3e25-5b58-8718-a08e67968179"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("54a45487-e51f-53bd-9c70-87529905cd36"),
                Level = "N5",
                Word = "何",
                Reading = "なん/なに",
                Meaning = "Cái gì",
                OrderIndex = 16,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("6e0c9738-e15e-502e-bc49-017fbd26db1d"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("d01bd13d-d6ae-5471-9ee2-0345fb64c34f"),
                Level = "N5",
                Word = "何月",
                Reading = "なんがつ",
                Meaning = "tháng mấy",
                OrderIndex = 17,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("78d67df1-50b7-57ce-8b97-05b899d11a6d"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("54a45487-e51f-53bd-9c70-87529905cd36"),
                Level = "N5",
                Word = "何人",
                Reading = "なんにん",
                Meaning = "mấy người",
                OrderIndex = 18,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("cc3001e3-be65-54df-a3c4-51ae81582a7c"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("54a45487-e51f-53bd-9c70-87529905cd36"),
                Level = "N5",
                Word = "何曜日",
                Reading = "なんようび",
                Meaning = "Thứ mấy",
                OrderIndex = 19,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("3b7f6356-e24a-5abb-b50c-c956b18a3aaa"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("54a45487-e51f-53bd-9c70-87529905cd36"),
                Level = "N5",
                Word = "何年",
                Reading = "なんねん",
                Meaning = "Năm bao nhiêu",
                OrderIndex = 20,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("8e1f8556-95f0-59fe-bfc6-cc6e6fff2f81"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("2473b071-dd7a-561b-9b5a-67dabe649765"),
                Level = "N5",
                Word = "今年",
                Reading = "ことし",
                Meaning = "Năm nay",
                OrderIndex = 21,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("dd35f64e-a882-5156-8976-dddb72cdc739"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("eff056fa-5214-5e6d-b2a3-c677dc8655fc"),
                Level = "N5",
                Word = "九時",
                Reading = "くじ",
                Meaning = "9 giờ",
                OrderIndex = 22,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("a51d249a-0dfb-54cc-9eb3-bba55fd858b5"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("54a45487-e51f-53bd-9c70-87529905cd36"),
                Level = "N5",
                Word = "何時",
                Reading = "なんじ",
                Meaning = "Mấy giờ",
                OrderIndex = 23,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("48e22b70-d933-576f-91d2-4dd2e3af9710"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("eff056fa-5214-5e6d-b2a3-c677dc8655fc"),
                Level = "N5",
                Word = "時間",
                Reading = "じかん",
                Meaning = "Thời gian",
                OrderIndex = 24,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("1bee6f8d-ff3a-50a4-8d0f-248bc515afe1"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("54a45487-e51f-53bd-9c70-87529905cd36"),
                Level = "N5",
                Word = "何時間",
                Reading = "なんじかん",
                Meaning = "Mấy tiếng",
                OrderIndex = 25,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("52425a55-871f-5c4a-89e6-e3bd22bca5c9"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("77624de6-8922-5e86-aa9a-824acd663ed6"),
                Level = "N5",
                Word = "分かります",
                Reading = "わかります",
                Meaning = "Hiểu",
                OrderIndex = 26,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("c37bdf0c-1424-5961-b1d7-fb4994326401"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("77624de6-8922-5e86-aa9a-824acd663ed6"),
                Level = "N5",
                Word = "半分",
                Reading = "はんぶん",
                Meaning = "1/2",
                OrderIndex = 27,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("b5497a56-fa9d-580a-9764-6382b857d3dc"),
                LessonId = lesson3Id,
                KanjiItemId = Guid.Parse("77624de6-8922-5e86-aa9a-824acd663ed6"),
                Level = "N5",
                Word = "六分",
                Reading = "ろっぷん",
                Meaning = "6 phút",
                OrderIndex = 28,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
        };

        foreach (var item in vocabItems3)
        {
            if (!await db.KanjiVocabularyItems.AnyAsync(v => v.Word == item.Word && v.Reading == item.Reading))
                db.KanjiVocabularyItems.Add(item);
        }

        // Lesson 4: Địa điểm và Phương hướng (JPD123)
        var lesson4Id = Guid.Parse("dbebdd02-f36c-58da-9c3f-8335e2f73e18");
        if (!await db.KanjiLessons.AnyAsync(l => l.Id == lesson4Id))
        {
            db.KanjiLessons.Add(new KanjiLesson { Id = lesson4Id, Level = "N3", LessonNumber = 4, Title = "Địa điểm và Phương hướng", Description = "Địa điểm và Phương hướng - JPD123 Kanji N3.", AccessTier = "free", PackageCode = "kanji_jpd123", OrderIndex = 4, CreatedAt = SeededAt, UpdatedAt = SeededAt });
        }

        var kanjiItems4 = new List<KanjiItem>
        {
            new KanjiItem {
                Id = Guid.Parse("ad4d2793-71e4-556e-a0f3-0ebaf4447021"),
                LessonId = lesson4Id,
                Level = "N3",
                Character = "東",
                HanViet = "ĐÔNG",
                Meaning = "Phía Đông",
                StrokeCount = 8,
                KunReading = "ひがし",
                OnReading = "トウ",
                Mnemonic = "Mặt trời (日) ló sau cây (木) ở phía đông (東).",
                ComponentMapJson = @"[{""character"": ""日"", ""component"": ""日"", ""name"": ""mặt trời"", ""meaning"": ""mặt trời""}, {""character"": ""木"", ""component"": ""木"", ""name"": ""cây"", ""meaning"": ""cây""}]",
                StrokeDataJson = @"[""M30.63,25.23c2.36,0.62,4.86,0.47,7.25,0.22c8.24-0.86,22.7-2.7,32.4-3.57c2.38-0.21,4.51-0.14,6.85,0.22"", ""M26.77,37.86c1.03,1.03,1.78,2.05,2.07,3.44c0.86,4.14,3.61,16.02,4.97,21.91c0.43,1.87,0.72,3.14,0.76,3.36"", ""M29.55,39.31c14.7-2.12,34.45-4.37,48.18-5.5c2.89-0.24,4.02,2.01,3.49,4.2c-1.33,5.48-2.84,12.21-5.27,19.87c-0.52,1.65-1.08,3.3-1.7,4.94"", ""M32.25,51.07c8.12-0.88,37.75-4.12,45.57-4.4"", ""M35.76,63.99c8.99-1.05,28.37-2.68,38.3-3.23"", ""M51.25,12.32c1.5,1.5,2.25,3.5,2.25,5.25c0,4.5,0.06,55.21-0.14,75.75c-0.04,3.7-0.07,5.29-0.11,6.25"", ""M51.62,63.94c-0.24,1.91-0.81,2.76-1.27,3.45c-6.59,9.83-20.19,21.12-31.6,26.42"", ""M55,64.44c7.5,7.2,21.77,17.49,29.78,22.16c2.81,1.64,6.1,3.51,9.34,4.09""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_06771"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:06771"" kvg:element=""東"">
	<g id=""kvg:06771-g1"" kvg:element=""木"" kvg:part=""1"" kvg:radical=""tradit"">
		<path id=""kvg:06771-s1"" kvg:type=""㇐"" d=""M30.63,25.23c2.36,0.62,4.86,0.47,7.25,0.22c8.24-0.86,22.7-2.7,32.4-3.57c2.38-0.21,4.51-0.14,6.85,0.22""/>
	</g>
	<g id=""kvg:06771-g2"" kvg:element=""日"">
		<path id=""kvg:06771-s2"" kvg:type=""㇑"" d=""M26.77,37.86c1.03,1.03,1.78,2.05,2.07,3.44c0.86,4.14,3.61,16.02,4.97,21.91c0.43,1.87,0.72,3.14,0.76,3.36""/>
		<path id=""kvg:06771-s3"" kvg:type=""㇕a"" d=""M29.55,39.31c14.7-2.12,34.45-4.37,48.18-5.5c2.89-0.24,4.02,2.01,3.49,4.2c-1.33,5.48-2.84,12.21-5.27,19.87c-0.52,1.65-1.08,3.3-1.7,4.94""/>
		<path id=""kvg:06771-s4"" kvg:type=""㇐a"" d=""M32.25,51.07c8.12-0.88,37.75-4.12,45.57-4.4""/>
		<path id=""kvg:06771-s5"" kvg:type=""㇐a"" d=""M35.76,63.99c8.99-1.05,28.37-2.68,38.3-3.23""/>
	</g>
	<g id=""kvg:06771-g3"" kvg:element=""木"" kvg:part=""2"" kvg:radical=""tradit"">
		<path id=""kvg:06771-s6"" kvg:type=""㇑"" d=""M51.25,12.32c1.5,1.5,2.25,3.5,2.25,5.25c0,4.5,0.06,55.21-0.14,75.75c-0.04,3.7-0.07,5.29-0.11,6.25""/>
		<g id=""kvg:06771-g4"" kvg:element=""丿"" kvg:radical=""nelson"">
			<path id=""kvg:06771-s7"" kvg:type=""㇒"" d=""M51.62,63.94c-0.24,1.91-0.81,2.76-1.27,3.45c-6.59,9.83-20.19,21.12-31.6,26.42""/>
		</g>
		<path id=""kvg:06771-s8"" kvg:type=""㇏"" d=""M55,64.44c7.5,7.2,21.77,17.49,29.78,22.16c2.81,1.64,6.1,3.51,9.34,4.09""/>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_06771"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 23.25 21.70)"">1</text>
	<text transform=""matrix(1 0 0 1 17.25 45.70)"">2</text>
	<text transform=""matrix(1 0 0 1 30.75 36.70)"">3</text>
	<text transform=""matrix(1 0 0 1 35.25 48.70)"">4</text>
	<text transform=""matrix(1 0 0 1 38.25 60.70)"">5</text>
	<text transform=""matrix(1 0 0 1 45.75 6.70)"">6</text>
	<text transform=""matrix(1 0 0 1 33.75 78.70)"">7</text>
	<text transform=""matrix(1 0 0 1 71.25 74.20)"">8</text>
</g>
</svg>
",
                OrderIndex = 1,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("64d8c3c5-11f7-5719-aca2-6028ab33b401"),
                LessonId = lesson4Id,
                Level = "N3",
                Character = "京",
                HanViet = "KINH",
                Meaning = "Kinh đô, thủ đô",
                StrokeCount = 8,
                KunReading = "みやこ",
                OnReading = "キョウ、ケイ",
                Mnemonic = "Khu dân cư (口) dưới mái che (亠) tạo thành kinh đô (京).",
                ComponentMapJson = @"[{""character"": ""亠"", ""component"": ""亠"", ""name"": ""mái che"", ""meaning"": ""mái che""}, {""character"": ""口"", ""component"": ""口"", ""name"": ""khu dân cư"", ""meaning"": ""khu dân cư""}, {""character"": ""小"", ""component"": ""小"", ""name"": ""nhỏ"", ""meaning"": ""nhỏ""}]",
                StrokeDataJson = @"[""M51.44,10.92c1.26,1.26,2.06,2.71,2.06,4.81c0,2.95-0.14,7.52-0.14,9.32"", ""M15.5,29.55c3.14,0.45,6.19,0.83,9.45,0.35c21.67-3.15,42.67-5.78,58.93-6.57c3.66-0.18,5.53-0.04,8.26,0.5"", ""M30.3,41.57c1.06,1.06,1.73,2.17,1.87,2.7c0.84,3.2,1.83,9.56,2.92,15.71c0.21,1.18,0.43,1.85,0.66,3.01"", ""M33.27,43.15c12.23-1.9,31.7-4.88,37.48-5.4C73.54,37.5,76.12,40.08,75,43c-0.95,2.47-2.79,8.86-4.03,12.08"", ""M36.21,60.53c8.92-1.28,21.37-2.65,32.54-4.03c1.37-0.17,3.25-0.38,4.5-0.25"", ""M53.52,65.33c0.98,1.17,1.51,2.79,1.51,4.77c0,4.9-0.26,17.4-0.26,21.62c0,9.03-6.71,1-8.21-0.25"", ""M31.75,73.25c0.12,0.91-0.01,1.79-0.39,2.62c-1.54,3.89-6.74,10.21-13.86,15.38"", ""M76.12,73.5c5.14,4.19,11.06,10.83,13.38,16.25""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_04eac"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:04eac"" kvg:element=""京"">
	<g id=""kvg:04eac-g1"" kvg:element=""亠"" kvg:position=""top"" kvg:radical=""general"">
		<path id=""kvg:04eac-s1"" kvg:type=""㇑a"" d=""M51.44,10.92c1.26,1.26,2.06,2.71,2.06,4.81c0,2.95-0.14,7.52-0.14,9.32""/>
		<path id=""kvg:04eac-s2"" kvg:type=""㇐"" d=""M15.5,29.55c3.14,0.45,6.19,0.83,9.45,0.35c21.67-3.15,42.67-5.78,58.93-6.57c3.66-0.18,5.53-0.04,8.26,0.5""/>
	</g>
	<g id=""kvg:04eac-g2"" kvg:position=""bottom"">
		<g id=""kvg:04eac-g3"" kvg:element=""口"">
			<path id=""kvg:04eac-s3"" kvg:type=""㇑"" d=""M30.3,41.57c1.06,1.06,1.73,2.17,1.87,2.7c0.84,3.2,1.83,9.56,2.92,15.71c0.21,1.18,0.43,1.85,0.66,3.01""/>
			<path id=""kvg:04eac-s4"" kvg:type=""㇕b"" d=""M33.27,43.15c12.23-1.9,31.7-4.88,37.48-5.4C73.54,37.5,76.12,40.08,75,43c-0.95,2.47-2.79,8.86-4.03,12.08""/>
			<path id=""kvg:04eac-s5"" kvg:type=""㇐b"" d=""M36.21,60.53c8.92-1.28,21.37-2.65,32.54-4.03c1.37-0.17,3.25-0.38,4.5-0.25""/>
		</g>
		<g id=""kvg:04eac-g4"" kvg:element=""小"">
			<path id=""kvg:04eac-s6"" kvg:type=""㇚"" d=""M53.52,65.33c0.98,1.17,1.51,2.79,1.51,4.77c0,4.9-0.26,17.4-0.26,21.62c0,9.03-6.71,1-8.21-0.25""/>
			<path id=""kvg:04eac-s7"" kvg:type=""㇒"" d=""M31.75,73.25c0.12,0.91-0.01,1.79-0.39,2.62c-1.54,3.89-6.74,10.21-13.86,15.38""/>
			<path id=""kvg:04eac-s8"" kvg:type=""㇔"" d=""M76.12,73.5c5.14,4.19,11.06,10.83,13.38,16.25""/>
		</g>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_04eac"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 45.75 7.63)"">1</text>
	<text transform=""matrix(1 0 0 1 8.25 30.13)"">2</text>
	<text transform=""matrix(1 0 0 1 24.75 51.13)"">3</text>
	<text transform=""matrix(1 0 0 1 32.25 39.13)"">4</text>
	<text transform=""matrix(1 0 0 1 39.75 57.13)"">5</text>
	<text transform=""matrix(1 0 0 1 45.75 73.63)"">6</text>
	<text transform=""matrix(1 0 0 1 23.25 70.63)"">7</text>
	<text transform=""matrix(1 0 0 1 74.25 69.13)"">8</text>
</g>
</svg>
",
                OrderIndex = 2,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("2daea2c1-2fc4-51dd-8d60-20539c5ed6b2"),
                LessonId = lesson4Id,
                Level = "N3",
                Character = "名",
                HanViet = "DANH",
                Meaning = "Tên gọi",
                StrokeCount = 6,
                KunReading = "な",
                OnReading = "メイ、ミョウ",
                Mnemonic = "Buổi tối (夕) dùng miệng (口) gọi tên nhau thành danh (名).",
                ComponentMapJson = @"[{""character"": ""夕"", ""component"": ""夕"", ""name"": ""buổi tối"", ""meaning"": ""buổi tối""}, {""character"": ""口"", ""component"": ""口"", ""name"": ""miệng"", ""meaning"": ""miệng""}]",
                StrokeDataJson = @"[""M54.2,12.64c0.3,1.61-0.07,2.99-0.69,4.24c-3.49,7.13-13.28,19.29-25.96,27.54"", ""M53.25,24.16c0.88,0.47,1.95,0.5,3.28,0.37c4.37-0.43,11.99-2.47,17.81-4.1c4.18-1.17,5.46,1.02,4.41,3.51C72.25,39.38,43.88,69.62,16,77.5"", ""M43.62,40.88c3.62,2,8,6,9.68,9.58"", ""M42,67.81c0.91,0.91,1.62,2.19,1.83,3.33c0.5,2.82,2.15,14.38,3.05,20.86c0.3,2.12,0.52,3.7,0.59,4.25"", ""M44.53,69.52c10.82-1.38,32.39-4.4,38.01-4.53c2.76-0.06,4.08,1.63,3.25,4.64c-1.13,4.06-3.52,14.04-4.64,20.36"", ""M47.99,93.99c7.26-0.61,19.65-1.61,29.54-2.37c2.19-0.17,4.24-0.28,6.04-0.31""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_0540d"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:0540d"" kvg:element=""名"">
	<g id=""kvg:0540d-g1"" kvg:element=""夕"" kvg:position=""top"" kvg:radical=""nelson"">
		<path id=""kvg:0540d-s1"" kvg:type=""㇒"" d=""M54.2,12.64c0.3,1.61-0.07,2.99-0.69,4.24c-3.49,7.13-13.28,19.29-25.96,27.54""/>
		<path id=""kvg:0540d-s2"" kvg:type=""㇇"" d=""M53.25,24.16c0.88,0.47,1.95,0.5,3.28,0.37c4.37-0.43,11.99-2.47,17.81-4.1c4.18-1.17,5.46,1.02,4.41,3.51C72.25,39.38,43.88,69.62,16,77.5""/>
		<path id=""kvg:0540d-s3"" kvg:type=""㇔"" d=""M43.62,40.88c3.62,2,8,6,9.68,9.58""/>
	</g>
	<g id=""kvg:0540d-g2"" kvg:element=""口"" kvg:position=""bottom"" kvg:radical=""tradit"">
		<path id=""kvg:0540d-s4"" kvg:type=""㇑"" d=""M42,67.81c0.91,0.91,1.62,2.19,1.83,3.33c0.5,2.82,2.15,14.38,3.05,20.86c0.3,2.12,0.52,3.7,0.59,4.25""/>
		<path id=""kvg:0540d-s5"" kvg:type=""㇕b"" d=""M44.53,69.52c10.82-1.38,32.39-4.4,38.01-4.53c2.76-0.06,4.08,1.63,3.25,4.64c-1.13,4.06-3.52,14.04-4.64,20.36""/>
		<path id=""kvg:0540d-s6"" kvg:type=""㇐b"" d=""M47.99,93.99c7.26-0.61,19.65-1.61,29.54-2.37c2.19-0.17,4.24-0.28,6.04-0.31""/>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_0540d"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 46.50 12.50)"">1</text>
	<text transform=""matrix(1 0 0 1 60.50 20.50)"">2</text>
	<text transform=""matrix(1 0 0 1 38.50 48.50)"">3</text>
	<text transform=""matrix(1 0 0 1 37.50 77.50)"">4</text>
	<text transform=""matrix(1 0 0 1 54.50 64.50)"">5</text>
	<text transform=""matrix(1 0 0 1 50.50 90.50)"">6</text>
</g>
</svg>
",
                OrderIndex = 3,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("65a224f0-0273-56ce-a540-8f4458b7304a"),
                LessonId = lesson4Id,
                Level = "N3",
                Character = "前",
                HanViet = "TIỀN",
                Meaning = "Phía trước",
                StrokeCount = 9,
                KunReading = "まえ",
                OnReading = "ゼン",
                Mnemonic = "Cầm dao (刂) hướng về phía trước cơ thể (月) tạo thành trước mặt (前).",
                ComponentMapJson = @"[{""character"": ""月"", ""component"": ""月"", ""name"": ""cơ thể / thịt"", ""meaning"": ""cơ thể / thịt""}, {""character"": ""刂"", ""component"": ""刂"", ""name"": ""dao"", ""meaning"": ""dao""}]",
                StrokeDataJson = @"[""M33.5,14.5c3.34,2.07,8.64,8.5,9.48,11.72"", ""M72.67,12c0.26,1.14,0.07,2.23-0.52,3.19c-2.12,3.41-6.02,8.65-8.6,11.31"", ""M13.38,33.24c2.63,0.72,7.46,0.94,10.08,0.72c21.67-1.83,39.8-3.58,62.68-4.61c4.37-0.2,7.01,0.34,9.2,0.7"", ""M24.65,45.46c1.1,1.29,1.63,2.92,1.63,4.17c0,3.05,0.1,28.65,0.08,40.62c0,3.23-0.03,5.46-0.1,6"", ""M27.01,46.8c2.3-0.41,13.37-2.3,17.2-2.9c2.2-0.34,3.51,1.1,3.51,3.24c0,1.02-0.16,30.82-0.16,44.63c0,5.48-3.79,2.98-5.93,0.53"", ""M27.65,60.01c5.72-0.76,13.63-1.81,18.59-2.32"", ""M27.57,73.47c4.41-0.51,13.3-1.54,18.4-1.89"", ""M62.27,47.58c1.2,1.2,1.76,2.67,1.76,4.58c0,8.93-0.01,15.1-0.09,18.59c-0.04,1.49-0.09,2.76-0.16,4.05"", ""M78.85,39.25c1.26,1.26,2.01,2.88,2.01,5.02c0,14.56-0.01,42.91-0.01,47.87c0,8.62-5.96,1-7.46-0.25""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_0524d"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:0524d"" kvg:element=""前"">
	<g id=""kvg:0524d-g1"" kvg:position=""top"" kvg:phon=""歬1V"">
		<g id=""kvg:0524d-g2"" kvg:element=""八"" kvg:variant=""true"" kvg:radical=""nelson"">
			<path id=""kvg:0524d-s1"" kvg:type=""㇔"" d=""M33.5,14.5c3.34,2.07,8.64,8.5,9.48,11.72""/>
			<path id=""kvg:0524d-s2"" kvg:type=""㇒"" d=""M72.67,12c0.26,1.14,0.07,2.23-0.52,3.19c-2.12,3.41-6.02,8.65-8.6,11.31""/>
		</g>
		<path id=""kvg:0524d-s3"" kvg:type=""㇐"" d=""M13.38,33.24c2.63,0.72,7.46,0.94,10.08,0.72c21.67-1.83,39.8-3.58,62.68-4.61c4.37-0.2,7.01,0.34,9.2,0.7""/>
	</g>
	<g id=""kvg:0524d-g3"" kvg:position=""bottom"">
		<g id=""kvg:0524d-g4"" kvg:element=""月"" kvg:variant=""true"" kvg:phon=""歬2V"">
			<path id=""kvg:0524d-s4"" kvg:type=""㇑"" d=""M24.65,45.46c1.1,1.29,1.63,2.92,1.63,4.17c0,3.05,0.1,28.65,0.08,40.62c0,3.23-0.03,5.46-0.1,6""/>
			<path id=""kvg:0524d-s5"" kvg:type=""㇆a"" d=""M27.01,46.8c2.3-0.41,13.37-2.3,17.2-2.9c2.2-0.34,3.51,1.1,3.51,3.24c0,1.02-0.16,30.82-0.16,44.63c0,5.48-3.79,2.98-5.93,0.53""/>
			<path id=""kvg:0524d-s6"" kvg:type=""㇐a"" d=""M27.65,60.01c5.72-0.76,13.63-1.81,18.59-2.32""/>
			<path id=""kvg:0524d-s7"" kvg:type=""㇐a"" d=""M27.57,73.47c4.41-0.51,13.3-1.54,18.4-1.89""/>
		</g>
		<g id=""kvg:0524d-g5"" kvg:element=""刂"" kvg:variant=""true"" kvg:original=""刀"" kvg:radical=""tradit"">
			<path id=""kvg:0524d-s8"" kvg:type=""㇑"" d=""M62.27,47.58c1.2,1.2,1.76,2.67,1.76,4.58c0,8.93-0.01,15.1-0.09,18.59c-0.04,1.49-0.09,2.76-0.16,4.05""/>
			<path id=""kvg:0524d-s9"" kvg:type=""㇚"" d=""M78.85,39.25c1.26,1.26,2.01,2.88,2.01,5.02c0,14.56-0.01,42.91-0.01,47.87c0,8.62-5.96,1-7.46-0.25""/>
		</g>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_0524d"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 26.25 13.50)"">1</text>
	<text transform=""matrix(1 0 0 1 63.50 10.50)"">2</text>
	<text transform=""matrix(1 0 0 1 6.50 34.50)"">3</text>
	<text transform=""matrix(1 0 0 1 18.50 52.63)"">4</text>
	<text transform=""matrix(1 0 0 1 27.50 44.50)"">5</text>
	<text transform=""matrix(1 0 0 1 29.50 57.13)"">6</text>
	<text transform=""matrix(1 0 0 1 29.50 72.50)"">7</text>
	<text transform=""matrix(1 0 0 1 56.25 45.50)"">8</text>
	<text transform=""matrix(1 0 0 1 70.50 42.50)"">9</text>
</g>
</svg>
",
                OrderIndex = 4,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("cf563118-7293-503e-b9b2-1828e82103ee"),
                LessonId = lesson4Id,
                Level = "N3",
                Character = "国",
                HanViet = "QUỐC",
                Meaning = "Quốc gia",
                StrokeCount = 8,
                KunReading = "くに",
                OnReading = "コク",
                Mnemonic = "Vua (王) nằm trong lãnh thổ bao quanh (囗) tạo thành quốc gia (国).",
                ComponentMapJson = @"[{""character"": ""囗"", ""component"": ""囗"", ""name"": ""khung bao"", ""meaning"": ""khung bao""}, {""character"": ""王"", ""component"": ""王"", ""name"": ""vua / ngọc"", ""meaning"": ""vua / ngọc""}]",
                StrokeDataJson = @"[""M19,16.82c1.09,1.09,1.61,2.51,1.61,4.41c0,14.65-0.22,44.9-0.22,71.53c0,1.95-0.06,3.86-0.09,5.75"", ""M21.52,18.67C41.38,16.75,74.03,13.5,85,13.5c3.38,0,5,1.85,5,5.25c0,15.36-0.04,47.89-0.08,70.62c0,1.68,0,3.31,0,4.88"", ""M35.12,33.88c1.32,0.28,4.2,0.44,5.51,0.28c10.47-1.29,20.62-2.54,28.62-3.13c2.02-0.15,3.88-0.19,5.56,0.04"", ""M52.8,34.89c0.96,0.97,1.47,2.48,1.47,3.81c0,3.99-0.13,24.74-0.09,33.55"", ""M37.58,52.16c1.79,0.22,3.41,0.14,5.36-0.08c7.56-0.83,17.56-1.99,25.38-2.8c1.25-0.13,4.02-0.15,5.89,0.17"", ""M31.08,75.14c1.54,0.36,3.85,0.28,5.19,0.16c9.98-0.92,25.85-2.67,37.03-3.54c2.15-0.17,5.04-0.18,6.12,0.13"", ""M67.75,56.62c3,1.75,6.12,5.12,8,8.38"", ""M21.5,93.01c14.25-0.51,48.38-1.89,67-2.51""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_056fd"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:056fd"" kvg:element=""国"">
	<g id=""kvg:056fd-g1"" kvg:element=""囗"" kvg:part=""1"" kvg:position=""kamae"" kvg:radical=""general"">
		<path id=""kvg:056fd-s1"" kvg:type=""㇑"" d=""M19,16.82c1.09,1.09,1.61,2.51,1.61,4.41c0,14.65-0.22,44.9-0.22,71.53c0,1.95-0.06,3.86-0.09,5.75""/>
		<path id=""kvg:056fd-s2"" kvg:type=""㇕a"" d=""M21.52,18.67C41.38,16.75,74.03,13.5,85,13.5c3.38,0,5,1.85,5,5.25c0,15.36-0.04,47.89-0.08,70.62c0,1.68,0,3.31,0,4.88""/>
	</g>
	<g id=""kvg:056fd-g2"" kvg:element=""玉"" kvg:phon=""或V"">
		<g id=""kvg:056fd-g3"" kvg:element=""王"" kvg:original=""玉"" kvg:partial=""true"">
			<path id=""kvg:056fd-s3"" kvg:type=""㇐"" d=""M35.12,33.88c1.32,0.28,4.2,0.44,5.51,0.28c10.47-1.29,20.62-2.54,28.62-3.13c2.02-0.15,3.88-0.19,5.56,0.04""/>
			<path id=""kvg:056fd-s4"" kvg:type=""㇑a"" d=""M52.8,34.89c0.96,0.97,1.47,2.48,1.47,3.81c0,3.99-0.13,24.74-0.09,33.55""/>
			<path id=""kvg:056fd-s5"" kvg:type=""㇐"" d=""M37.58,52.16c1.79,0.22,3.41,0.14,5.36-0.08c7.56-0.83,17.56-1.99,25.38-2.8c1.25-0.13,4.02-0.15,5.89,0.17""/>
			<path id=""kvg:056fd-s6"" kvg:type=""㇐"" d=""M31.08,75.14c1.54,0.36,3.85,0.28,5.19,0.16c9.98-0.92,25.85-2.67,37.03-3.54c2.15-0.17,5.04-0.18,6.12,0.13""/>
		</g>
		<g id=""kvg:056fd-g4"" kvg:element=""丶"">
			<path id=""kvg:056fd-s7"" kvg:type=""㇔"" d=""M67.75,56.62c3,1.75,6.12,5.12,8,8.38""/>
		</g>
	</g>
	<g id=""kvg:056fd-g5"" kvg:element=""囗"" kvg:part=""2"" kvg:position=""kamae"" kvg:radical=""general"">
		<path id=""kvg:056fd-s8"" kvg:type=""㇐a"" d=""M21.5,93.01c14.25-0.51,48.38-1.89,67-2.51""/>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_056fd"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 12.50 25.63)"">1</text>
	<text transform=""matrix(1 0 0 1 22.50 14.50)"">2</text>
	<text transform=""matrix(1 0 0 1 27.75 35.50)"">3</text>
	<text transform=""matrix(1 0 0 1 45.75 43.50)"">4</text>
	<text transform=""matrix(1 0 0 1 30.50 54.13)"">5</text>
	<text transform=""matrix(1 0 0 1 29.25 70.63)"">6</text>
	<text transform=""matrix(1 0 0 1 77.25 60.13)"">7</text>
	<text transform=""matrix(1 0 0 1 25.50 88.63)"">8</text>
</g>
</svg>
",
                OrderIndex = 5,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("56840c05-f8d1-5b3d-bcdc-7284a7d660ef"),
                LessonId = lesson4Id,
                Level = "N3",
                Character = "南",
                HanViet = "NAM",
                Meaning = "Phía Nam",
                StrokeCount = 9,
                KunReading = "みなみ",
                OnReading = "ナン",
                Mnemonic = "Vùng đất bao quanh (冂) phía nam (南) có nhiều người tụ họp dưới chữ mười (十).",
                ComponentMapJson = @"[{""character"": ""十"", ""component"": ""十"", ""name"": ""mười"", ""meaning"": ""mười""}, {""character"": ""冂"", ""component"": ""冂"", ""name"": ""vùng bao"", ""meaning"": ""vùng bao""}]",
                StrokeDataJson = @"[""M27.38,28.25c1.82,0.38,4.2,0.45,7.22,0.2c11.52-0.95,28.37-3.2,40.58-3.7c3.03-0.12,4.92-0.23,6.82,0"", ""M52.35,10.75c0.95,0.95,1.55,2.86,1.55,3.96c0,7.3,0,19.47,0,28.54"", ""M20.25,45.75c1.25,1.25,1.89,2.74,2,5c0.19,4.06,0.83,27.03,1.12,37.99c0.08,3.23,0.13,5.48,0.13,6.01"", ""M22.78,47.6c16.31-1.76,59.32-6.35,60.97-6.35c5,0,6.25,1.62,6.25,6.75c0,5.25-0.25,37.3-0.25,43.05c0,7.7-3.5,6.45-9.5,0.95"", ""M38.5,49.38c2.58,1.74,6.66,7.14,7.31,9.84"", ""M66.25,45.5c0.05,0.89-0.07,1.75-0.37,2.59c-0.8,2.73-2.75,6.92-5.26,9.66"", ""M34.78,61.67c1.81,0.39,4.55,0.55,6.36,0.39c9.11-0.8,18.63-1.54,27.63-2.39c2.99-0.28,4.83-0.32,6.33-0.12"", ""M33.13,74.19c1.81,0.49,4.55,0.83,6.38,0.74c10.73-0.56,22.66-1.94,31.04-2.66c3-0.26,4.82-0.02,6.33,0.23"", ""M53.5,62.5c0.75,0.75,1,2.14,0.99,3.5c-0.04,6.22-0.15,18.46-0.21,25.26c-0.02,2.19-0.03,3.81-0.03,4.49""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_05357"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:05357"" kvg:element=""南"">
	<g id=""kvg:05357-g1"" kvg:element=""十"" kvg:position=""top"" kvg:radical=""general"">
		<path id=""kvg:05357-s1"" kvg:type=""㇐"" d=""M27.38,28.25c1.82,0.38,4.2,0.45,7.22,0.2c11.52-0.95,28.37-3.2,40.58-3.7c3.03-0.12,4.92-0.23,6.82,0""/>
		<path id=""kvg:05357-s2"" kvg:type=""㇑a"" d=""M52.35,10.75c0.95,0.95,1.55,2.86,1.55,3.96c0,7.3,0,19.47,0,28.54""/>
	</g>
	<g id=""kvg:05357-g2"" kvg:position=""bottom"">
		<g id=""kvg:05357-g3"" kvg:element=""冂"">
			<path id=""kvg:05357-s3"" kvg:type=""㇑"" d=""M20.25,45.75c1.25,1.25,1.89,2.74,2,5c0.19,4.06,0.83,27.03,1.12,37.99c0.08,3.23,0.13,5.48,0.13,6.01""/>
			<path id=""kvg:05357-s4"" kvg:type=""㇆a"" d=""M22.78,47.6c16.31-1.76,59.32-6.35,60.97-6.35c5,0,6.25,1.62,6.25,6.75c0,5.25-0.25,37.3-0.25,43.05c0,7.7-3.5,6.45-9.5,0.95""/>
		</g>
		<path id=""kvg:05357-s5"" kvg:type=""㇔"" d=""M38.5,49.38c2.58,1.74,6.66,7.14,7.31,9.84""/>
		<path id=""kvg:05357-s6"" kvg:type=""㇒"" d=""M66.25,45.5c0.05,0.89-0.07,1.75-0.37,2.59c-0.8,2.73-2.75,6.92-5.26,9.66""/>
		<g id=""kvg:05357-g4"" kvg:element=""干"">
			<path id=""kvg:05357-s7"" kvg:type=""㇐"" d=""M34.78,61.67c1.81,0.39,4.55,0.55,6.36,0.39c9.11-0.8,18.63-1.54,27.63-2.39c2.99-0.28,4.83-0.32,6.33-0.12""/>
			<g id=""kvg:05357-g5"" kvg:element=""十"">
				<path id=""kvg:05357-s8"" kvg:type=""㇐"" d=""M33.13,74.19c1.81,0.49,4.55,0.83,6.38,0.74c10.73-0.56,22.66-1.94,31.04-2.66c3-0.26,4.82-0.02,6.33,0.23""/>
				<path id=""kvg:05357-s9"" kvg:type=""㇑"" d=""M53.5,62.5c0.75,0.75,1,2.14,0.99,3.5c-0.04,6.22-0.15,18.46-0.21,25.26c-0.02,2.19-0.03,3.81-0.03,4.49""/>
			</g>
		</g>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_05357"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 20.50 29.50)"">1</text>
	<text transform=""matrix(1 0 0 1 44.00 11.63)"">2</text>
	<text transform=""matrix(1 0 0 1 14.50 57.50)"">3</text>
	<text transform=""matrix(1 0 0 1 23.50 44.50)"">4</text>
	<text transform=""matrix(1 0 0 1 32.50 55.50)"">5</text>
	<text transform=""matrix(1 0 0 1 56.25 52.50)"">6</text>
	<text transform=""matrix(1 0 0 1 28.50 63.50)"">7</text>
	<text transform=""matrix(1 0 0 1 27.50 71.50)"">8</text>
	<text transform=""matrix(1 0 0 1 47.50 70.50)"">9</text>
</g>
</svg>
",
                OrderIndex = 6,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("a44d56c2-f6a7-5d8b-826c-059016900958"),
                LessonId = lesson4Id,
                Level = "N3",
                Character = "女",
                HanViet = "NỮ",
                Meaning = "Phụ nữ",
                StrokeCount = 3,
                KunReading = "おんな",
                OnReading = "ジョ、ニョ",
                Mnemonic = "Hình dáng người phụ nữ quỳ tạo thành chữ nữ (女).",
                ComponentMapJson = @"[]",
                StrokeDataJson = @"[""M53.21,18.37c0.54,2.13,0.26,3.41-0.25,5.25C50.38,33,42.62,52.75,35.75,64c-1.39,2.27-1,3.5,1,3.5c11.63,0,28.46,7.48,38.83,16.41c2.56,2.21,4.68,4.51,6.17,6.84"", ""M69.62,42.18c0.5,1.7,0.63,3.57-0.01,5.93C65.93,61.8,54.61,81.6,27,91.75"", ""M13.88,50.43c3.48,1.39,7.26,0.85,10.88,0.53c19.52-1.7,42.04-4.08,60.61-4.63c3.66-0.11,7.21-0.1,10.62,1.42""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_05973"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:05973"" kvg:element=""女"" kvg:radical=""general"">
	<path id=""kvg:05973-s1"" kvg:type=""㇛"" d=""M53.21,18.37c0.54,2.13,0.26,3.41-0.25,5.25C50.38,33,42.62,52.75,35.75,64c-1.39,2.27-1,3.5,1,3.5c11.63,0,28.46,7.48,38.83,16.41c2.56,2.21,4.68,4.51,6.17,6.84""/>
	<path id=""kvg:05973-s2"" kvg:type=""㇒"" d=""M69.62,42.18c0.5,1.7,0.63,3.57-0.01,5.93C65.93,61.8,54.61,81.6,27,91.75""/>
	<path id=""kvg:05973-s3"" kvg:type=""㇐"" d=""M13.88,50.43c3.48,1.39,7.26,0.85,10.88,0.53c19.52-1.7,42.04-4.08,60.61-4.63c3.66-0.11,7.21-0.1,10.62,1.42""/>
</g>
</g>
<g id=""kvg:StrokeNumbers_05973"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 45.00 17.50)"">1</text>
	<text transform=""matrix(1 0 0 1 68.50 37.50)"">2</text>
	<text transform=""matrix(1 0 0 1 5.50 51.50)"">3</text>
</g>
</svg>
",
                OrderIndex = 7,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("1828b3e7-255d-54ee-80d6-b2972cf5b7a5"),
                LessonId = lesson4Id,
                Level = "N3",
                Character = "男",
                HanViet = "NAM",
                Meaning = "Nam giới",
                StrokeCount = 7,
                KunReading = "おとこ",
                OnReading = "ダン、ナン",
                Mnemonic = "Dùng sức mạnh (力) làm ruộng (田) là đàn ông (男).",
                ComponentMapJson = @"[{""character"": ""田"", ""component"": ""田"", ""name"": ""ruộng"", ""meaning"": ""ruộng""}, {""character"": ""力"", ""component"": ""力"", ""name"": ""sức mạnh"", ""meaning"": ""sức mạnh""}]",
                StrokeDataJson = @"[""M26.5,14.25c0.88,0.88,1.56,1.99,1.73,2.98c0.84,4.77,2.47,16.75,3.34,26.04c0.18,1.95,0.37,2.37,0.55,4.23"", ""M29,15.95c11.38-1.45,41.21-4.57,49.56-4.71c3.9-0.07,5.44,1.51,4.91,5.29c-0.45,3.21-3.15,15.19-4.94,22.23c-0.41,1.62-0.79,2.99-1.4,4.19"", ""M54,15.97c0.77,0.77,1,1.91,1,2.79c0.02,6.32,0.2,22,0.2,22.75"", ""M30.98,30.82C45,29.12,57.12,28,80.53,26.34"", ""M32.87,44.74c11.38-1.24,28.38-2.99,44.14-3.7"", ""M19.98,60.98c2.15,0.67,4.58,0.78,6.77,0.53c13.46-1.53,42.24-5.66,51.88-6.86c5.26-0.66,6.86,1.04,5.72,6.27c-1.92,8.83-9,27.39-15.66,33.19c-5.11,4.45-7.44,2.14-9.69-0.86"", ""M53.22,46.43c0.28,1.32,0.29,3.04-0.2,4.57C49.12,63.12,38,81.25,17.14,92.06""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_07537"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:07537"" kvg:element=""男"">
	<g id=""kvg:07537-g1"" kvg:element=""田"" kvg:position=""top"" kvg:radical=""general"">
		<path id=""kvg:07537-s1"" kvg:type=""㇑"" d=""M26.5,14.25c0.88,0.88,1.56,1.99,1.73,2.98c0.84,4.77,2.47,16.75,3.34,26.04c0.18,1.95,0.37,2.37,0.55,4.23""/>
		<path id=""kvg:07537-s2"" kvg:type=""㇕a"" d=""M29,15.95c11.38-1.45,41.21-4.57,49.56-4.71c3.9-0.07,5.44,1.51,4.91,5.29c-0.45,3.21-3.15,15.19-4.94,22.23c-0.41,1.62-0.79,2.99-1.4,4.19""/>
		<path id=""kvg:07537-s3"" kvg:type=""㇑a"" d=""M54,15.97c0.77,0.77,1,1.91,1,2.79c0.02,6.32,0.2,22,0.2,22.75""/>
		<path id=""kvg:07537-s4"" kvg:type=""㇐a"" d=""M30.98,30.82C45,29.12,57.12,28,80.53,26.34""/>
		<path id=""kvg:07537-s5"" kvg:type=""㇐a"" d=""M32.87,44.74c11.38-1.24,28.38-2.99,44.14-3.7""/>
	</g>
	<g id=""kvg:07537-g2"" kvg:element=""力"" kvg:position=""bottom"">
		<path id=""kvg:07537-s6"" kvg:type=""㇆"" d=""M19.98,60.98c2.15,0.67,4.58,0.78,6.77,0.53c13.46-1.53,42.24-5.66,51.88-6.86c5.26-0.66,6.86,1.04,5.72,6.27c-1.92,8.83-9,27.39-15.66,33.19c-5.11,4.45-7.44,2.14-9.69-0.86""/>
		<path id=""kvg:07537-s7"" kvg:type=""㇒"" d=""M53.22,46.43c0.28,1.32,0.29,3.04-0.2,4.57C49.12,63.12,38,81.25,17.14,92.06""/>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_07537"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 18.75 19.50)"">1</text>
	<text transform=""matrix(1 0 0 1 31.50 12.50)"">2</text>
	<text transform=""matrix(1 0 0 1 47.50 24.13)"">3</text>
	<text transform=""matrix(1 0 0 1 34.00 27.50)"">4</text>
	<text transform=""matrix(1 0 0 1 34.50 41.50)"">5</text>
	<text transform=""matrix(1 0 0 1 11.50 63.50)"">6</text>
	<text transform=""matrix(1 0 0 1 45.50 52.50)"">7</text>
</g>
</svg>
",
                OrderIndex = 8,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("eab57142-b7f2-5ae2-86b2-142856a3c4a0"),
                LessonId = lesson4Id,
                Level = "N3",
                Character = "区",
                HanViet = "KHU",
                Meaning = "Khu vực",
                StrokeCount = 4,
                KunReading = "",
                OnReading = "ク",
                Mnemonic = "Không gian được chia (㐅) trong khung (匚) thành khu vực (区).",
                ComponentMapJson = @"[{""character"": ""匚"", ""component"": ""匚"", ""name"": ""khung mở"", ""meaning"": ""khung mở""}, {""character"": ""㐅"", ""component"": ""㐅"", ""name"": ""phân chia"", ""meaning"": ""phân chia""}]",
                StrokeDataJson = @"[""M18.88,23.93c2.78,0.65,5.74,0.51,8.58,0.17c18.07-2.16,35.69-3.8,51.98-5.95c3.2-0.42,5.17-0.25,6.79,0.02"", ""M68.57,30.43c0.25,1.37,0.29,2.74-0.47,4.76C62,51.38,49.62,64.75,32.9,75.75"", ""M35.75,43.25C51.07,47.69,70.34,63.1,78,72.5"", ""M20.36,25.3c0.73,0.73,1,2.18,1,3.7c0,11.28,0,48.74,0,57.34c0,4.4,0.77,5.33,5.02,4.74c16.58-2.31,44.87-3.94,59.2-3.66c3.34,0.07,6.17,0.23,8.23,0.68""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_0533a"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:0533a"" kvg:element=""区"">
	<g id=""kvg:0533a-g1"" kvg:element=""匸"" kvg:part=""1"" kvg:position=""kamae"" kvg:radical=""general"">
		<path id=""kvg:0533a-s1"" kvg:type=""㇐"" d=""M18.88,23.93c2.78,0.65,5.74,0.51,8.58,0.17c18.07-2.16,35.69-3.8,51.98-5.95c3.2-0.42,5.17-0.25,6.79,0.02""/>
	</g>
	<g id=""kvg:0533a-g2"" kvg:element=""乂"">
		<g id=""kvg:0533a-g3"" kvg:element=""丿"">
			<path id=""kvg:0533a-s2"" kvg:type=""㇒"" d=""M68.57,30.43c0.25,1.37,0.29,2.74-0.47,4.76C62,51.38,49.62,64.75,32.9,75.75""/>
		</g>
		<path id=""kvg:0533a-s3"" kvg:type=""㇔/㇏"" d=""M35.75,43.25C51.07,47.69,70.34,63.1,78,72.5""/>
	</g>
	<g id=""kvg:0533a-g4"" kvg:element=""匸"" kvg:part=""2"" kvg:position=""kamae"" kvg:radical=""general"">
		<path id=""kvg:0533a-s4"" kvg:type=""㇗"" d=""M20.36,25.3c0.73,0.73,1,2.18,1,3.7c0,11.28,0,48.74,0,57.34c0,4.4,0.77,5.33,5.02,4.74c16.58-2.31,44.87-3.94,59.2-3.66c3.34,0.07,6.17,0.23,8.23,0.68""/>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_0533a"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 23.50 20.50)"">1</text>
	<text transform=""matrix(1 0 0 1 60.50 30.50)"">2</text>
	<text transform=""matrix(1 0 0 1 31.50 40.50)"">3</text>
	<text transform=""matrix(1 0 0 1 14.50 32.50)"">4</text>
</g>
</svg>
",
                OrderIndex = 9,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("53a1dfd9-c926-58fc-b1fe-b56a4e64375e"),
                LessonId = lesson4Id,
                Level = "N3",
                Character = "市",
                HanViet = "THỊ",
                Meaning = "Thành phố / chợ",
                StrokeCount = 5,
                KunReading = "いち",
                OnReading = "シ",
                Mnemonic = "Những tấm vải (巾) dưới mái che (亠) tạo nên khu chợ, thành thị (市).",
                ComponentMapJson = @"[{""character"": ""亠"", ""component"": ""亠"", ""name"": ""mái che"", ""meaning"": ""mái che""}, {""character"": ""巾"", ""component"": ""巾"", ""name"": ""vải / khăn"", ""meaning"": ""vải / khăn""}]",
                StrokeDataJson = @"[""M52.86,12c1.26,1.26,1.48,2.5,1.48,4.66c0,4.84-0.15,6.46-0.15,8.9"", ""M15.5,28.96c3.65,0.76,7.24,0.69,9.36,0.46c16.74-1.8,45.31-4.8,60.95-5.6c3.62-0.19,5.48,0.04,8.18,0.59"", ""M27.31,46c1.22,1.22,1.6,2.75,1.6,4.61c0,3.34-0.06,19.8-0.16,28.65c-0.02,2.13-0.05,3.8-0.08,4.72"", ""M30.28,48.5c12.28-1.24,45.6-4.38,48.37-4.38c3.6,0,5.35,0.87,5.35,5.09c0,9.3-0.25,21.16-1.4,29.97c-1.06,8.11-6.24,0.06-7.73-1.27"", ""M53.53,32.88c1.17,1.17,1.85,2.87,1.85,4.36c0,5.51-0.27,35.65-0.32,54.26c-0.01,2.28-0.01,4.31-0.01,6""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_05e02"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:05e02"" kvg:element=""市"">
	<g id=""kvg:05e02-g1"" kvg:element=""亠"" kvg:position=""top"" kvg:radical=""nelson"">
		<path id=""kvg:05e02-s1"" kvg:type=""㇑a"" d=""M52.86,12c1.26,1.26,1.48,2.5,1.48,4.66c0,4.84-0.15,6.46-0.15,8.9""/>
		<path id=""kvg:05e02-s2"" kvg:type=""㇐"" d=""M15.5,28.96c3.65,0.76,7.24,0.69,9.36,0.46c16.74-1.8,45.31-4.8,60.95-5.6c3.62-0.19,5.48,0.04,8.18,0.59""/>
	</g>
	<g id=""kvg:05e02-g2"" kvg:element=""巾"" kvg:position=""bottom"" kvg:radical=""tradit"" kvg:phon=""止V"">
		<path id=""kvg:05e02-s3"" kvg:type=""㇑"" d=""M27.31,46c1.22,1.22,1.6,2.75,1.6,4.61c0,3.34-0.06,19.8-0.16,28.65c-0.02,2.13-0.05,3.8-0.08,4.72""/>
		<path id=""kvg:05e02-s4"" kvg:type=""㇆a"" d=""M30.28,48.5c12.28-1.24,45.6-4.38,48.37-4.38c3.6,0,5.35,0.87,5.35,5.09c0,9.3-0.25,21.16-1.4,29.97c-1.06,8.11-6.24,0.06-7.73-1.27""/>
		<path id=""kvg:05e02-s5"" kvg:type=""㇑"" d=""M53.53,32.88c1.17,1.17,1.85,2.87,1.85,4.36c0,5.51-0.27,35.65-0.32,54.26c-0.01,2.28-0.01,4.31-0.01,6""/>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_05e02"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 47 9.13)"">1</text>
	<text transform=""matrix(1 0 0 1 8.25 28.63)"">2</text>
	<text transform=""matrix(1 0 0 1 18.75 49.63)"">3</text>
	<text transform=""matrix(1 0 0 1 30 45)"">4</text>
	<text transform=""matrix(1 0 0 1 57 36)"">5</text>
</g>
</svg>
",
                OrderIndex = 10,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
        };

        foreach (var item in kanjiItems4)
        {
            if (!await db.KanjiItems.AnyAsync(k => k.Character == item.Character))
                db.KanjiItems.Add(item);
        }

        var vocabItems4 = new List<KanjiVocabulary>
        {
            new KanjiVocabulary {
                Id = Guid.Parse("38246502-81c1-55d7-800b-ab6038f2f9fc"),
                LessonId = lesson4Id,
                KanjiItemId = Guid.Parse("ad4d2793-71e4-556e-a0f3-0ebaf4447021"),
                Level = "N3",
                Word = "東",
                Reading = "ひがし",
                Meaning = "phía đông",
                OrderIndex = 1,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("2eab0700-9e82-58e0-99e2-5749bf6f468b"),
                LessonId = lesson4Id,
                KanjiItemId = Guid.Parse("ad4d2793-71e4-556e-a0f3-0ebaf4447021"),
                Level = "N3",
                Word = "東京",
                Reading = "とうきょう",
                Meaning = "Tokyo",
                OrderIndex = 2,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("9a885a75-435b-577a-8401-cfb2c202a0bb"),
                LessonId = lesson4Id,
                KanjiItemId = Guid.Parse("65a224f0-0273-56ce-a540-8f4458b7304a"),
                Level = "N3",
                Word = "午前",
                Reading = "ごぜん",
                Meaning = "a.m",
                OrderIndex = 3,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("b70898fd-3a3c-55ce-ac41-ddb100637e13"),
                LessonId = lesson4Id,
                KanjiItemId = Guid.Parse("65a224f0-0273-56ce-a540-8f4458b7304a"),
                Level = "N3",
                Word = "前日",
                Reading = "ぜんじつ",
                Meaning = "ngày trước đó",
                OrderIndex = 4,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("62fa9519-d830-5954-baba-dd2673ea3b7b"),
                LessonId = lesson4Id,
                KanjiItemId = Guid.Parse("2daea2c1-2fc4-51dd-8d60-20539c5ed6b2"),
                Level = "N3",
                Word = "名前",
                Reading = "なまえ",
                Meaning = "tên",
                OrderIndex = 5,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("b34c1852-6eba-5054-abc6-fd7552893535"),
                LessonId = lesson4Id,
                KanjiItemId = Guid.Parse("cf563118-7293-503e-b9b2-1828e82103ee"),
                Level = "N3",
                Word = "国",
                Reading = "くに",
                Meaning = "đất nước, quốc gia",
                OrderIndex = 6,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("8052d08b-11f8-5206-8116-bb614a917811"),
                LessonId = lesson4Id,
                KanjiItemId = Guid.Parse("cf563118-7293-503e-b9b2-1828e82103ee"),
                Level = "N3",
                Word = "外国",
                Reading = "がいこく",
                Meaning = "nước ngoài",
                OrderIndex = 7,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("565889bb-d243-5bbb-8902-358013696892"),
                LessonId = lesson4Id,
                KanjiItemId = Guid.Parse("1828b3e7-255d-54ee-80d6-b2972cf5b7a5"),
                Level = "N3",
                Word = "男の人",
                Reading = "おとこのひと",
                Meaning = "người đàn ông",
                OrderIndex = 8,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("2b894757-e270-5cc5-82a9-627a1f431f93"),
                LessonId = lesson4Id,
                KanjiItemId = Guid.Parse("a44d56c2-f6a7-5d8b-826c-059016900958"),
                Level = "N3",
                Word = "女の人",
                Reading = "おんなのひと",
                Meaning = "người phụ nữ",
                OrderIndex = 9,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("912c2820-2c4c-55c2-be32-bdfa47830ba8"),
                LessonId = lesson4Id,
                KanjiItemId = Guid.Parse("eab57142-b7f2-5ae2-86b2-142856a3c4a0"),
                Level = "N3",
                Word = "～区",
                Reading = "〜く",
                Meaning = "quận ~",
                OrderIndex = 10,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("ac34fcf5-b055-58cf-bc16-ffc3151b9587"),
                LessonId = lesson4Id,
                KanjiItemId = Guid.Parse("53a1dfd9-c926-58fc-b1fe-b56a4e64375e"),
                Level = "N3",
                Word = "～市",
                Reading = "〜し",
                Meaning = "Thành phố ~",
                OrderIndex = 11,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("02a077ae-757b-5e67-91e9-5deb88fb2ac8"),
                LessonId = lesson4Id,
                KanjiItemId = Guid.Parse("65a224f0-0273-56ce-a540-8f4458b7304a"),
                Level = "N3",
                Word = "前",
                Reading = "まえ",
                Meaning = "phía trước",
                OrderIndex = 12,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("60864bcf-a671-51e7-af63-394fe6a9adc1"),
                LessonId = lesson4Id,
                KanjiItemId = Guid.Parse("64d8c3c5-11f7-5719-aca2-6028ab33b401"),
                Level = "N3",
                Word = "京都",
                Reading = "きょうと",
                Meaning = "Kyoto",
                OrderIndex = 13,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("b44ad0ba-048a-5563-943d-cc1fe21025a7"),
                LessonId = lesson4Id,
                KanjiItemId = Guid.Parse("cf563118-7293-503e-b9b2-1828e82103ee"),
                Level = "N3",
                Word = "外国人",
                Reading = "がいこくじん",
                Meaning = "người nước ngoài",
                OrderIndex = 14,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("1d66bea9-1afc-5c33-9a63-df608a15af8a"),
                LessonId = lesson4Id,
                KanjiItemId = Guid.Parse("a44d56c2-f6a7-5d8b-826c-059016900958"),
                Level = "N3",
                Word = "男女",
                Reading = "だんじょ",
                Meaning = "nam nữ",
                OrderIndex = 15,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
        };

        foreach (var item in vocabItems4)
        {
            if (!await db.KanjiVocabularyItems.AnyAsync(v => v.Word == item.Word && v.Reading == item.Reading))
                db.KanjiVocabularyItems.Add(item);
        }

        // Lesson 5: Hành động và Nghỉ ngơi (JPD123)
        var lesson5Id = Guid.Parse("d3096310-4ab4-55d7-9084-720334835ccc");
        if (!await db.KanjiLessons.AnyAsync(l => l.Id == lesson5Id))
        {
            db.KanjiLessons.Add(new KanjiLesson { Id = lesson5Id, Level = "N3", LessonNumber = 5, Title = "Hành động và Nghỉ ngơi", Description = "Hành động và Nghỉ ngơi - JPD123 Kanji N3.", AccessTier = "free", PackageCode = "kanji_jpd123", OrderIndex = 5, CreatedAt = SeededAt, UpdatedAt = SeededAt });
        }

        var kanjiItems5 = new List<KanjiItem>
        {
            new KanjiItem {
                Id = Guid.Parse("f8571dc2-b128-582f-afe8-56183685f8d5"),
                LessonId = lesson5Id,
                Level = "N3",
                Character = "先",
                HanViet = "TIÊN",
                Meaning = "Trước, đầu tiên",
                StrokeCount = 6,
                KunReading = "さき",
                OnReading = "セン",
                Mnemonic = "Con trâu (⺧) mọc hai đôi chân người (儿) chạy vượt lên trước tiên.",
                ComponentMapJson = @"[{""character"": ""⺧"", ""component"": ""⺧"", ""name"": ""⺧"", ""meaning"": ""⺧""}, {""character"": ""儿"", ""component"": ""儿"", ""name"": ""儿"", ""meaning"": ""儿""}]",
                StrokeDataJson = @"[""M37.51,21c0.07,0.62,0.15,1.61-0.14,2.49C35.25,29.88,31.62,37.38,24.5,45"", ""M38.13,32.04c1.5,0.09,3.95-0.16,4.64-0.22c6.48-0.57,20.36-1.82,27.82-2.94c1.65-0.25,3.66-0.13,5.16,0.27"", ""M52.81,12.38c1.28,1.28,2.01,3.12,2.01,4.75c0,0.75-0.05,31.92-0.07,32.87"", ""M15.88,53.26c3.42,0.98,7.15,0.5,10.62,0.22c15.99-1.3,38.99-3.55,59-4.4c2.94-0.13,5.84-0.03,8.75,0.47"", ""M45.18,55.68c0.32,1.45,0.15,2.48-0.15,3.85C43.24,67.65,35,86.62,20,96.38"", ""M60.49,53.62c1.07,1.07,1.38,2.71,1.38,4.98c0,7.78-0.22,14.88-0.22,21.89c0,15.14,1.1,16.04,15.85,16.04c14.62,0,15.64-1.78,15.64-11.29""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_05148"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:05148"" kvg:element=""先"">
	<g id=""kvg:05148-g1"" kvg:position=""top"">
		<path id=""kvg:05148-s1"" kvg:type=""㇒"" d=""M37.51,21c0.07,0.62,0.15,1.61-0.14,2.49C35.25,29.88,31.62,37.38,24.5,45""/>
		<path id=""kvg:05148-s2"" kvg:type=""㇐"" d=""M38.13,32.04c1.5,0.09,3.95-0.16,4.64-0.22c6.48-0.57,20.36-1.82,27.82-2.94c1.65-0.25,3.66-0.13,5.16,0.27""/>
		<path id=""kvg:05148-s3"" kvg:type=""㇑a"" d=""M52.81,12.38c1.28,1.28,2.01,3.12,2.01,4.75c0,0.75-0.05,31.92-0.07,32.87""/>
		<path id=""kvg:05148-s4"" kvg:type=""㇐"" d=""M15.88,53.26c3.42,0.98,7.15,0.5,10.62,0.22c15.99-1.3,38.99-3.55,59-4.4c2.94-0.13,5.84-0.03,8.75,0.47""/>
	</g>
	<g id=""kvg:05148-g2"" kvg:element=""儿"" kvg:original=""八"" kvg:position=""bottom"" kvg:radical=""general"">
		<g id=""kvg:05148-g3"" kvg:element=""丿"">
			<path id=""kvg:05148-s5"" kvg:type=""㇒"" d=""M45.18,55.68c0.32,1.45,0.15,2.48-0.15,3.85C43.24,67.65,35,86.62,20,96.38""/>
		</g>
		<path id=""kvg:05148-s6"" kvg:type=""㇟"" d=""M60.49,53.62c1.07,1.07,1.38,2.71,1.38,4.98c0,7.78-0.22,14.88-0.22,21.89c0,15.14,1.1,16.04,15.85,16.04c14.62,0,15.64-1.78,15.64-11.29""/>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_05148"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 29.50 20.13)"">1</text>
	<text transform=""matrix(1 0 0 1 42.50 28.50)"">2</text>
	<text transform=""matrix(1 0 0 1 43.50 11.50)"">3</text>
	<text transform=""matrix(1 0 0 1 7.50 54.50)"">4</text>
	<text transform=""matrix(1 0 0 1 36.75 62.13)"">5</text>
	<text transform=""matrix(1 0 0 1 53.50 65.50)"">6</text>
</g>
</svg>
",
                OrderIndex = 1,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("1d97f9ed-5faa-57cf-bf8b-3adb806a8f0b"),
                LessonId = lesson5Id,
                Level = "N3",
                Character = "週",
                HanViet = "CHU",
                Meaning = "Tuần lễ",
                StrokeCount = 11,
                KunReading = "",
                OnReading = "シュウ",
                Mnemonic = "Bước đi (⻌) trọn vẹn một vòng chu kỳ (周) là kết thúc một tuần lễ.",
                ComponentMapJson = @"[{""character"": ""⻌"", ""component"": ""⻌"", ""name"": ""⻌"", ""meaning"": ""⻌""}, {""character"": ""周"", ""component"": ""周"", ""name"": ""周"", ""meaning"": ""周""}]",
                StrokeDataJson = @"[""M45.93,17.66c1.02,1.02,1.67,2.55,1.67,4.3c0,21.04,2.53,45.54-8.1,58.79"", ""M48.49,18.95c9.79-1.25,32.29-4.39,33.41-4.45c4.34-0.24,5.42,1.26,5.42,5.64c0,1.93,0.17,52.16,0.17,56.6c0,9.53-5.28,4.86-8.4,1.69"", ""M55.43,31.42c0.83,0.25,2.88,0.54,3.72,0.51c4.54-0.18,13.51-1.92,16.84-2.23c1.39-0.13,2.22-0.07,2.91,0.03"", ""M65.84,22.3c0.99,0.99,1.51,2.32,1.51,3.39c0,3.19-0.07,10.44-0.07,16.81"", ""M53.61,44.53c1,0.35,2.64,0.41,4.31,0.23C65,44,70.38,43.38,77.01,42.4c1.84-0.27,3.25-0.4,4.81-0.26"", ""M56.17,53.75c1.08,0.75,1.6,2.01,1.71,2.97c0.5,4.53,1.05,6.63,1.48,10.27c0.17,1.46,0.3,2.58,0.34,2.94"", ""M58.6,55.4c5.86-1.39,13.69-2.83,16.29-3.02c1.73-0.13,2.48,1,2.28,2.58c-0.38,2.92-1.34,5.94-2.12,9.61"", ""M60.65,67.64c2.32-0.44,8.3-1.1,13.1-1.63c1.43-0.16,2.69-0.28,3.61-0.34"", ""M20.71,19c3.1,1.41,8.02,5.8,8.79,8"", ""M13.25,54.95c2.25,0.92,4.29,0.84,5.25,0.48c2.5-0.93,8.31-4.06,9.75-4.68c2.88-1.24,4.14,0.9,1.5,3.78c-6.38,6.98-6,8.23-0.75,14.1c1.83,2.04,2.03,3.44-1.5,6.12c-5.25,4-7.5,5.75-10.75,8.5"", ""M13.75,85.75c4.12-0.88,10.41-0.97,15-0.5c7.25,0.75,29.97,5.13,34.5,6c13,2.5,21.25,4.5,30.25,2.75""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_09031"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:09031"" kvg:element=""週"">
	<g id=""kvg:09031-g1"" kvg:element=""周"" kvg:position=""nyoc"" kvg:phon=""周"">
		<g id=""kvg:09031-g2"" kvg:element=""冂"" kvg:variant=""true"">
			<path id=""kvg:09031-s1"" kvg:type=""㇒/㇑"" d=""M45.93,17.66c1.02,1.02,1.67,2.55,1.67,4.3c0,21.04,2.53,45.54-8.1,58.79""/>
			<path id=""kvg:09031-s2"" kvg:type=""㇆a"" d=""M48.49,18.95c9.79-1.25,32.29-4.39,33.41-4.45c4.34-0.24,5.42,1.26,5.42,5.64c0,1.93,0.17,52.16,0.17,56.6c0,9.53-5.28,4.86-8.4,1.69""/>
		</g>
		<g id=""kvg:09031-g3"" kvg:element=""吉"">
			<g id=""kvg:09031-g4"" kvg:element=""士"">
				<path id=""kvg:09031-s3"" kvg:type=""㇐"" d=""M55.43,31.42c0.83,0.25,2.88,0.54,3.72,0.51c4.54-0.18,13.51-1.92,16.84-2.23c1.39-0.13,2.22-0.07,2.91,0.03""/>
				<path id=""kvg:09031-s4"" kvg:type=""㇑a"" d=""M65.84,22.3c0.99,0.99,1.51,2.32,1.51,3.39c0,3.19-0.07,10.44-0.07,16.81""/>
				<path id=""kvg:09031-s5"" kvg:type=""㇐"" d=""M53.61,44.53c1,0.35,2.64,0.41,4.31,0.23C65,44,70.38,43.38,77.01,42.4c1.84-0.27,3.25-0.4,4.81-0.26""/>
			</g>
			<g id=""kvg:09031-g5"" kvg:element=""口"">
				<path id=""kvg:09031-s6"" kvg:type=""㇑"" d=""M56.17,53.75c1.08,0.75,1.6,2.01,1.71,2.97c0.5,4.53,1.05,6.63,1.48,10.27c0.17,1.46,0.3,2.58,0.34,2.94""/>
				<path id=""kvg:09031-s7"" kvg:type=""㇕b"" d=""M58.6,55.4c5.86-1.39,13.69-2.83,16.29-3.02c1.73-0.13,2.48,1,2.28,2.58c-0.38,2.92-1.34,5.94-2.12,9.61""/>
				<path id=""kvg:09031-s8"" kvg:type=""㇐b"" d=""M60.65,67.64c2.32-0.44,8.3-1.1,13.1-1.63c1.43-0.16,2.69-0.28,3.61-0.34""/>
			</g>
		</g>
	</g>
	<g id=""kvg:09031-g6"" kvg:element=""⻌"" kvg:original=""辶"" kvg:position=""nyo"" kvg:radical=""general"">
		<path id=""kvg:09031-s9"" kvg:type=""㇔"" d=""M20.71,19c3.1,1.41,8.02,5.8,8.79,8""/>
		<path id=""kvg:09031-s10"" kvg:type=""㇋"" d=""M13.25,54.95c2.25,0.92,4.29,0.84,5.25,0.48c2.5-0.93,8.31-4.06,9.75-4.68c2.88-1.24,4.14,0.9,1.5,3.78c-6.38,6.98-6,8.23-0.75,14.1c1.83,2.04,2.03,3.44-1.5,6.12c-5.25,4-7.5,5.75-10.75,8.5""/>
		<path id=""kvg:09031-s11"" kvg:type=""㇏a"" d=""M13.75,85.75c4.12-0.88,10.41-0.97,15-0.5c7.25,0.75,29.97,5.13,34.5,6c13,2.5,21.25,4.5,30.25,2.75""/>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_09031"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 39.75 28.63)"">1</text>
	<text transform=""matrix(1 0 0 1 53.25 13.63)"">2</text>
	<text transform=""matrix(1 0 0 1 54.75 28.63)"">3</text>
	<text transform=""matrix(1 0 0 1 69.75 24.13)"">4</text>
	<text transform=""matrix(1 0 0 1 51.75 40.63)"">5</text>
	<text transform=""matrix(1 0 0 1 50.25 66.13)"">6</text>
	<text transform=""matrix(1 0 0 1 60.75 54.13)"">7</text>
	<text transform=""matrix(1 0 0 1 62.25 64.63)"">8</text>
	<text transform=""matrix(1 0 0 1 11.25 16.63)"">9</text>
	<text transform=""matrix(1 0 0 1 0.75 52.63)"">10</text>
	<text transform=""matrix(1 0 0 1 2.25 85.63)"">11</text>
</g>
</svg>
",
                OrderIndex = 2,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("f719f4c9-db44-595f-8a75-f3530018ae3d"),
                LessonId = lesson5Id,
                Level = "N3",
                Character = "毎",
                HanViet = "MỖI",
                Meaning = "Mỗi, mọi",
                StrokeCount = 7,
                KunReading = "ごと",
                OnReading = "マイ",
                Mnemonic = "Người (𠂉) mẹ (毋) đội nón làm việc vất vả mỗi ngày.",
                ComponentMapJson = @"[{""character"": ""毋"", ""component"": ""毋"", ""name"": ""毋"", ""meaning"": ""毋""}]",
                StrokeDataJson = @"[""M44.13,10.62c0.11,1.39-0.04,2.48-0.54,3.78c-2.23,5.81-9.37,16.73-16.84,22.34"", ""M43.77,23.04c1.73,0.08,3.68,0.03,4.73-0.04c6.88-0.46,19.87-3.02,27.66-4.42c2.11-0.38,3.74-0.39,5.84-0.14"", ""M41.55,34.87c1.1,0.91,1.96,3.97,1.36,6.3C40.25,51.5,33.25,67,29.45,72.74c-1.98,2.99-0.52,4.58,1.57,4.62c11.98,0.26,25.1,1.88,37.95,5.72c5.92,1.77,11.28,4.6,15.77,7.92"", ""M44.26,37.32c9.17-1.07,22.11-2.57,28.58-3.32c3.9-0.45,6.36,2.01,6.1,5.12c-1.44,17.51-4.81,42.51-11.32,56.07c-2.99,6.22-6.5-1.15-7.52-2.27"", ""M58.71,39.5c0.51,1,0.75,2.74,0.53,4c-1.53,8.5-7.11,27-9.92,35"", ""M10.88,58.99c2.46,0.56,6.98,0.49,9.45,0.31c21.05-1.54,44.8-3.42,69.3-3.88c4.11-0.08,6.57,0.26,8.62,0.54""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_06bce"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:06bce"" kvg:element=""毎"">
	<g id=""kvg:06bce-g1"" kvg:element=""𠂉"" kvg:position=""top"">
		<g id=""kvg:06bce-g2"" kvg:element=""丿"">
			<path id=""kvg:06bce-s1"" kvg:type=""㇒"" d=""M44.13,10.62c0.11,1.39-0.04,2.48-0.54,3.78c-2.23,5.81-9.37,16.73-16.84,22.34""/>
		</g>
		<path id=""kvg:06bce-s2"" kvg:type=""㇐"" d=""M43.77,23.04c1.73,0.08,3.68,0.03,4.73-0.04c6.88-0.46,19.87-3.02,27.66-4.42c2.11-0.38,3.74-0.39,5.84-0.14""/>
	</g>
	<g id=""kvg:06bce-g3"" kvg:element=""毋"" kvg:partial=""true"" kvg:position=""bottom"" kvg:radical=""general"">
		<path id=""kvg:06bce-s3"" kvg:type=""㇛"" d=""M41.55,34.87c1.1,0.91,1.96,3.97,1.36,6.3C40.25,51.5,33.25,67,29.45,72.74c-1.98,2.99-0.52,4.58,1.57,4.62c11.98,0.26,25.1,1.88,37.95,5.72c5.92,1.77,11.28,4.6,15.77,7.92""/>
		<path id=""kvg:06bce-s4"" kvg:type=""㇆a"" d=""M44.26,37.32c9.17-1.07,22.11-2.57,28.58-3.32c3.9-0.45,6.36,2.01,6.1,5.12c-1.44,17.51-4.81,42.51-11.32,56.07c-2.99,6.22-6.5-1.15-7.52-2.27""/>
		<path id=""kvg:06bce-s5"" kvg:type=""㇑a"" d=""M58.71,39.5c0.51,1,0.75,2.74,0.53,4c-1.53,8.5-7.11,27-9.92,35""/>
		<path id=""kvg:06bce-s6"" kvg:type=""㇐"" d=""M10.88,58.99c2.46,0.56,6.98,0.49,9.45,0.31c21.05-1.54,44.8-3.42,69.3-3.88c4.11-0.08,6.57,0.26,8.62,0.54""/>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_06bce"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 37.50 10.50)"">1</text>
	<text transform=""matrix(1 0 0 1 50.50 19.50)"">2</text>
	<text transform=""matrix(1 0 0 1 34.50 43.50)"">3</text>
	<text transform=""matrix(1 0 0 1 45.50 33.50)"">4</text>
	<text transform=""matrix(1 0 0 1 51.50 45.50)"">5</text>
	<text transform=""matrix(1 0 0 1 3.50 60.50)"">6</text>
</g>
</svg>
",
                OrderIndex = 3,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("b1762e10-ea10-50cb-9513-ce389737a925"),
                LessonId = lesson5Id,
                Level = "N3",
                Character = "午",
                HanViet = "NGỌ",
                Meaning = "Buổi trưa, giờ Ngọ",
                StrokeCount = 4,
                KunReading = "",
                OnReading = "ゴ",
                Mnemonic = "Đúng mười (十) giờ sáng có một tia nắng chói chang (ノ) chiếu xuống báo hiệu sắp đến giờ Ngọ (trưa).",
                ComponentMapJson = @"[{""character"": ""十"", ""component"": ""十"", ""name"": ""十"", ""meaning"": ""十""}]",
                StrokeDataJson = @"[""M37.5,9.14c0.06,0.7,0.22,1.85-0.11,2.83C35.25,18.25,27,29.62,17.5,39"", ""M32.13,27.28c2.75,0.09,4.3-0.07,5.82-0.21c12.92-1.2,20.78-2.82,33.1-4.68c2.49-0.38,4.69-0.24,5.95,0.03"", ""M13.88,54.53c3,0.72,7.23,0.71,9.74,0.46c19.64-1.99,42.64-4.99,63-6.16c4.22-0.24,6.77,0.22,8.89,0.45"", ""M53.06,28.13c1.03,1.03,1.79,2.37,1.79,4.33c0,0.88-0.02,44.17-0.13,61.04c-0.02,2.88-0.03,4.96-0.05,5.88""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_05348"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:05348"" kvg:element=""午"">
	<g id=""kvg:05348-g1"" kvg:position=""top"">
		<g id=""kvg:05348-g2"" kvg:element=""丿"" kvg:radical=""nelson"">
			<path id=""kvg:05348-s1"" kvg:type=""㇒"" d=""M37.5,9.14c0.06,0.7,0.22,1.85-0.11,2.83C35.25,18.25,27,29.62,17.5,39""/>
		</g>
		<g id=""kvg:05348-g3"" kvg:element=""干"" kvg:part=""1"">
			<path id=""kvg:05348-s2"" kvg:type=""㇐"" d=""M32.13,27.28c2.75,0.09,4.3-0.07,5.82-0.21c12.92-1.2,20.78-2.82,33.1-4.68c2.49-0.38,4.69-0.24,5.95,0.03""/>
		</g>
	</g>
	<g id=""kvg:05348-g4"" kvg:element=""干"" kvg:part=""2"" kvg:position=""bottom"">
		<g id=""kvg:05348-g5"" kvg:element=""十"" kvg:radical=""tradit"">
			<path id=""kvg:05348-s3"" kvg:type=""㇐"" d=""M13.88,54.53c3,0.72,7.23,0.71,9.74,0.46c19.64-1.99,42.64-4.99,63-6.16c4.22-0.24,6.77,0.22,8.89,0.45""/>
			<path id=""kvg:05348-s4"" kvg:type=""㇑"" d=""M53.06,28.13c1.03,1.03,1.79,2.37,1.79,4.33c0,0.88-0.02,44.17-0.13,61.04c-0.02,2.88-0.03,4.96-0.05,5.88""/>
		</g>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_05348"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 29.50 9.50)"">1</text>
	<text transform=""matrix(1 0 0 1 40.50 23.50)"">2</text>
	<text transform=""matrix(1 0 0 1 6.50 57.50)"">3</text>
	<text transform=""matrix(1 0 0 1 46.50 35.50)"">4</text>
</g>
</svg>
",
                OrderIndex = 4,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("95b380a4-8169-5d38-b87a-bca35a0ac341"),
                LessonId = lesson5Id,
                Level = "N3",
                Character = "後",
                HanViet = "HẬU",
                Meaning = "Phía sau, sau này",
                StrokeCount = 9,
                KunReading = "あと、うしろ、のち",
                OnReading = "ゴ、コウ",
                Mnemonic = "Bước chân (彳) đi chậm chạp (夂) lùi lại phía sau, nhường đường cho đứa nhỏ (幺).",
                ComponentMapJson = @"[{""character"": ""彳"", ""component"": ""彳"", ""name"": ""彳"", ""meaning"": ""彳""}, {""character"": ""夂"", ""component"": ""夂"", ""name"": ""夂"", ""meaning"": ""夂""}, {""character"": ""幺"", ""component"": ""幺"", ""name"": ""幺"", ""meaning"": ""幺""}]",
                StrokeDataJson = @"[""M34.25,18.38c0,1.3-0.24,2.26-0.93,3.05c-3.57,4.07-8.94,8.7-15.91,13.39"", ""M38.75,36.62c0.14,1.32-0.42,2.67-1.13,3.79c-3.45,5.4-11.43,14.17-22.37,22.71"", ""M28.4,54.36c0.81,0.81,1.38,2.02,1.38,3.28c0,0.68,0.03,25.57-0.07,35.86c-0.02,1.74-0.04,3.05-0.05,3.75"", ""M61.16,12.62c0.29,1.07,0.21,2.43-0.39,3.54c-2.52,4.6-6.1,9.01-9.88,12.93c-1.01,1.05-1.26,2.17,0,2.68c2.96,1.19,6.3,3.11,8.88,5.07"", ""M76.35,20.12c0.27,1.25-0.3,2.56-1,3.31c-7.6,8.19-16.6,16.57-26.61,25.32c-1.25,1.1-0.74,1.76,0.74,1.41C55.72,48.69,74.5,44.4,82.5,43"", ""M77.88,36.25c3.66,2.21,9.46,9.07,10.38,12.5"", ""M57.75,53c0.09,1.13,0.02,2.27-0.4,3.33c-2.2,5.51-7.08,13.22-15.6,20.92"", ""M59.31,59.82c1.17,0.13,2.31,0.02,3.29-0.09c2.65-0.29,9.84-1.39,13.62-2.29c2.59-0.62,3.24,0.68,2.67,2.4C75.25,70.75,59.25,88.88,43.02,96.5"", ""M54.75,67.5c2.52,0,19.5,15.75,31.17,24.23c2.31,1.68,4.74,3.38,7.58,4.01""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_05f8c"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:05f8c"" kvg:element=""後"">
	<g id=""kvg:05f8c-g1"" kvg:element=""彳"" kvg:position=""left"" kvg:radical=""general"">
		<path id=""kvg:05f8c-s1"" kvg:type=""㇒"" d=""M34.25,18.38c0,1.3-0.24,2.26-0.93,3.05c-3.57,4.07-8.94,8.7-15.91,13.39""/>
		<g id=""kvg:05f8c-g2"" kvg:element=""亻"" kvg:variant=""true"" kvg:original=""人"">
			<path id=""kvg:05f8c-s2"" kvg:type=""㇒"" d=""M38.75,36.62c0.14,1.32-0.42,2.67-1.13,3.79c-3.45,5.4-11.43,14.17-22.37,22.71""/>
			<path id=""kvg:05f8c-s3"" kvg:type=""㇑"" d=""M28.4,54.36c0.81,0.81,1.38,2.02,1.38,3.28c0,0.68,0.03,25.57-0.07,35.86c-0.02,1.74-0.04,3.05-0.05,3.75""/>
		</g>
	</g>
	<g id=""kvg:05f8c-g3"" kvg:position=""right"">
		<g id=""kvg:05f8c-g4"" kvg:element=""幺"">
			<path id=""kvg:05f8c-s4"" kvg:type=""㇜"" d=""M61.16,12.62c0.29,1.07,0.21,2.43-0.39,3.54c-2.52,4.6-6.1,9.01-9.88,12.93c-1.01,1.05-1.26,2.17,0,2.68c2.96,1.19,6.3,3.11,8.88,5.07""/>
			<path id=""kvg:05f8c-s5"" kvg:type=""㇜"" d=""M76.35,20.12c0.27,1.25-0.3,2.56-1,3.31c-7.6,8.19-16.6,16.57-26.61,25.32c-1.25,1.1-0.74,1.76,0.74,1.41C55.72,48.69,74.5,44.4,82.5,43""/>
			<path id=""kvg:05f8c-s6"" kvg:type=""㇔"" d=""M77.88,36.25c3.66,2.21,9.46,9.07,10.38,12.5""/>
		</g>
		<g id=""kvg:05f8c-g5"" kvg:element=""夂"">
			<path id=""kvg:05f8c-s7"" kvg:type=""㇒"" d=""M57.75,53c0.09,1.13,0.02,2.27-0.4,3.33c-2.2,5.51-7.08,13.22-15.6,20.92""/>
			<path id=""kvg:05f8c-s8"" kvg:type=""㇇"" d=""M59.31,59.82c1.17,0.13,2.31,0.02,3.29-0.09c2.65-0.29,9.84-1.39,13.62-2.29c2.59-0.62,3.24,0.68,2.67,2.4C75.25,70.75,59.25,88.88,43.02,96.5""/>
			<path id=""kvg:05f8c-s9"" kvg:type=""㇏"" d=""M54.75,67.5c2.52,0,19.5,15.75,31.17,24.23c2.31,1.68,4.74,3.38,7.58,4.01""/>
		</g>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_05f8c"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 26.50 18.50)"">1</text>
	<text transform=""matrix(1 0 0 1 30.50 39.50)"">2</text>
	<text transform=""matrix(1 0 0 1 23.25 66.50)"">3</text>
	<text transform=""matrix(1 0 0 1 52.50 14.50)"">4</text>
	<text transform=""matrix(1 0 0 1 68.50 21.50)"">5</text>
	<text transform=""matrix(1 0 0 1 78.50 34.63)"">6</text>
	<text transform=""matrix(1 0 0 1 49.50 60.50)"">7</text>
	<text transform=""matrix(1 0 0 1 63.50 56.50)"">8</text>
	<text transform=""matrix(1 0 0 1 53.25 76.63)"">9</text>
</g>
</svg>
",
                OrderIndex = 5,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("a7b0cfdd-9764-55e2-a5f5-adf8d3f005f7"),
                LessonId = lesson5Id,
                Level = "N3",
                Character = "見",
                HanViet = "KIẾN",
                Meaning = "Nhìn, thấy",
                StrokeCount = 7,
                KunReading = "みる、みえる",
                OnReading = "ケン",
                Mnemonic = "Con mắt (目) mọc thêm hai cái chân (儿) chạy lon ton đi nhìn ngắm khắp nơi.",
                ComponentMapJson = @"[{""character"": ""目"", ""component"": ""目"", ""name"": ""目"", ""meaning"": ""目""}, {""character"": ""儿"", ""component"": ""儿"", ""name"": ""儿"", ""meaning"": ""儿""}]",
                StrokeDataJson = @"[""M32.5,15.46c0.96,0.96,1.18,2.1,1.18,3.52c0,1.12,0.07,27.43-0.02,39.27c-0.02,3.12-0.02,5.21,0.02,5.5"", ""M34.65,17.15c9.23-1.27,22.23-2.65,31.1-3.65c2.99-0.34,4.26,1.01,4.26,3.55c0,2.5-0.1,28.08-0.14,38.96c-0.01,2.91-0.02,4.75-0.02,4.79"", ""M34.84,31.1c7.28-0.6,25.03-2.98,33.9-3.38"", ""M34.86,44.63C43.38,44,59,42.12,68.6,41.51"", ""M34.71,59.66C44.5,59,58.38,57.5,68.45,57.03"", ""M41.99,66.75c0.26,1.5,0.01,2.99-0.41,4.04c-2.7,6.83-13.83,20.83-28.41,27.87"", ""M54.49,61.37c1.07,1.07,1.33,2.59,1.38,4.43c0.2,8.19,0.04,6.2,0.04,18.2c0,10.12,1.23,11.53,18.54,11.53c18.81,0,19.81-1.53,19.81-10.12""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_0898b"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:0898b"" kvg:element=""見"" kvg:radical=""general"">
	<g id=""kvg:0898b-g1"" kvg:element=""目"" kvg:position=""top"">
		<path id=""kvg:0898b-s1"" kvg:type=""㇑"" d=""M32.5,15.46c0.96,0.96,1.18,2.1,1.18,3.52c0,1.12,0.07,27.43-0.02,39.27c-0.02,3.12-0.02,5.21,0.02,5.5""/>
		<path id=""kvg:0898b-s2"" kvg:type=""㇕a"" d=""M34.65,17.15c9.23-1.27,22.23-2.65,31.1-3.65c2.99-0.34,4.26,1.01,4.26,3.55c0,2.5-0.1,28.08-0.14,38.96c-0.01,2.91-0.02,4.75-0.02,4.79""/>
		<path id=""kvg:0898b-s3"" kvg:type=""㇐a"" d=""M34.84,31.1c7.28-0.6,25.03-2.98,33.9-3.38""/>
		<path id=""kvg:0898b-s4"" kvg:type=""㇐a"" d=""M34.86,44.63C43.38,44,59,42.12,68.6,41.51""/>
		<path id=""kvg:0898b-s5"" kvg:type=""㇐a"" d=""M34.71,59.66C44.5,59,58.38,57.5,68.45,57.03""/>
	</g>
	<g id=""kvg:0898b-g2"" kvg:element=""儿"" kvg:position=""bottom"">
		<path id=""kvg:0898b-s6"" kvg:type=""㇒"" d=""M41.99,66.75c0.26,1.5,0.01,2.99-0.41,4.04c-2.7,6.83-13.83,20.83-28.41,27.87""/>
		<path id=""kvg:0898b-s7"" kvg:type=""㇟"" d=""M54.49,61.37c1.07,1.07,1.33,2.59,1.38,4.43c0.2,8.19,0.04,6.2,0.04,18.2c0,10.12,1.23,11.53,18.54,11.53c18.81,0,19.81-1.53,19.81-10.12""/>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_0898b"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 26.50 23.50)"">1</text>
	<text transform=""matrix(1 0 0 1 35.50 13.50)"">2</text>
	<text transform=""matrix(1 0 0 1 38.25 27.43)"">3</text>
	<text transform=""matrix(1 0 0 1 38.25 40.63)"">4</text>
	<text transform=""matrix(1 0 0 1 38.25 55.63)"">5</text>
	<text transform=""matrix(1 0 0 1 32.50 74.50)"">6</text>
	<text transform=""matrix(1 0 0 1 48.50 69.50)"">7</text>
</g>
</svg>
",
                OrderIndex = 6,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("d3679ee3-f082-5ca0-8920-cf1336840317"),
                LessonId = lesson5Id,
                Level = "N3",
                Character = "食",
                HanViet = "THỰC",
                Meaning = "Ăn, thức ăn",
                StrokeCount = 9,
                KunReading = "たべる",
                OnReading = "ショク、ジキ",
                Mnemonic = "Người (人) được dọn cho đồ ăn ngon (良) sẽ có một bữa thực (ăn) thịnh soạn.",
                ComponentMapJson = @"[{""character"": ""人"", ""component"": ""人"", ""name"": ""人"", ""meaning"": ""人""}, {""character"": ""良"", ""component"": ""良"", ""name"": ""良"", ""meaning"": ""良""}]",
                StrokeDataJson = @"[""M52.75,10.5c0.11,0.98-0.19,2.67-0.97,3.93C45,25.34,31.75,41.19,14,51.5"", ""M52.75,16.25c5.09,4.8,25.71,19.61,33.7,24.9c2.68,1.78,5.37,2.79,8.55,3.35"", ""M52.25,29.25c1,1,1.5,2.25,1.5,3.5c0,2,0,3,0,5.5"", ""M38,40c0.83,0.47,2.19,1,3.86,0.83c9.39-0.96,21.95-2.76,23.25-2.84c1.67-0.1,3.14,0.88,3.11,2.53C68.2,41.8,67,53.25,66.34,62.4c-0.07,0.94-0.13,1.36-0.13,1.99"", ""M40.83,51.73C47.25,51.25,59.5,50,66,49.75"", ""M40.69,63.9c7.04-0.52,16.55-1.62,24.6-2.04"", ""M38.25,40.25c1.12,1.12,1.5,2.62,1.5,4c0,9.12,0,43.62,0,47.25c0,4,1,4.88,4.12,2.88c2.93-1.87,6.75-5.25,10.88-8.38"", ""M74,64c0.25,1.25,0.09,2.57-0.75,3.5c-3.5,3.88-4.5,4.88-7.25,7.5"", ""M51.5,71C55.75,71,77,90,81,92.75c2.49,1.71,4.62,2.62,7.5,3.5""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_098df"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:098df"" kvg:element=""食"" kvg:radical=""general"">
	<path id=""kvg:098df-s1"" kvg:type=""㇒"" d=""M52.75,10.5c0.11,0.98-0.19,2.67-0.97,3.93C45,25.34,31.75,41.19,14,51.5""/>
	<path id=""kvg:098df-s2"" kvg:type=""㇏"" d=""M52.75,16.25c5.09,4.8,25.71,19.61,33.7,24.9c2.68,1.78,5.37,2.79,8.55,3.35""/>
	<path id=""kvg:098df-s3"" kvg:type=""㇑a"" d=""M52.25,29.25c1,1,1.5,2.25,1.5,3.5c0,2,0,3,0,5.5""/>
	<path id=""kvg:098df-s4"" kvg:type=""㇕"" d=""M38,40c0.83,0.47,2.19,1,3.86,0.83c9.39-0.96,21.95-2.76,23.25-2.84c1.67-0.1,3.14,0.88,3.11,2.53C68.2,41.8,67,53.25,66.34,62.4c-0.07,0.94-0.13,1.36-0.13,1.99""/>
	<path id=""kvg:098df-s5"" kvg:type=""㇐a"" d=""M40.83,51.73C47.25,51.25,59.5,50,66,49.75""/>
	<path id=""kvg:098df-s6"" kvg:type=""㇐a"" d=""M40.69,63.9c7.04-0.52,16.55-1.62,24.6-2.04""/>
	<path id=""kvg:098df-s7"" kvg:type=""㇙"" d=""M38.25,40.25c1.12,1.12,1.5,2.62,1.5,4c0,9.12,0,43.62,0,47.25c0,4,1,4.88,4.12,2.88c2.93-1.87,6.75-5.25,10.88-8.38""/>
	<path id=""kvg:098df-s8"" kvg:type=""㇒"" d=""M74,64c0.25,1.25,0.09,2.57-0.75,3.5c-3.5,3.88-4.5,4.88-7.25,7.5""/>
	<path id=""kvg:098df-s9"" kvg:type=""㇏"" d=""M51.5,71C55.75,71,77,90,81,92.75c2.49,1.71,4.62,2.62,7.5,3.5""/>
</g>
</g>
<g id=""kvg:StrokeNumbers_098df"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 44.25 10.50)"">1</text>
	<text transform=""matrix(1 0 0 1 61.50 19.63)"">2</text>
	<text transform=""matrix(1 0 0 1 51.50 26.50)"">3</text>
	<text transform=""matrix(1 0 0 1 43.50 37.50)"">4</text>
	<text transform=""matrix(1 0 0 1 43.50 49.63)"">5</text>
	<text transform=""matrix(1 0 0 1 43.50 61.63)"">6</text>
	<text transform=""matrix(1 0 0 1 32.25 51.13)"">7</text>
	<text transform=""matrix(1 0 0 1 77.50 63.13)"">8</text>
	<text transform=""matrix(1 0 0 1 48.50 78.50)"">9</text>
</g>
</svg>
",
                OrderIndex = 7,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("a03537e6-2ed4-5467-b66a-dc384eaa4687"),
                LessonId = lesson5Id,
                Level = "N3",
                Character = "飲",
                HanViet = "ẨM",
                Meaning = "Uống",
                StrokeCount = 12,
                KunReading = "のむ",
                OnReading = "イン",
                Mnemonic = "Khi cơ thể cảm thấy thiếu (欠) thốn, phải há miệng nạp ngay đồ ăn thức uống (飠).",
                ComponentMapJson = @"[{""character"": ""欠"", ""component"": ""欠"", ""name"": ""欠"", ""meaning"": ""欠""}, {""character"": ""飠"", ""component"": ""飠"", ""name"": ""飠"", ""meaning"": ""飠""}]",
                StrokeDataJson = @"[""M31.53,14.5c0.06,0.73,0.24,1.94-0.12,2.92c-2.96,8.1-11.79,20.97-21.04,27.95"", ""M33.5,19.5c5.48,2.15,11.9,6.21,15,10.75"", ""M32.79,32.26c0.85,0.85,1.28,2.38,1.28,3.32c0,3.54-0.08,5.92-0.08,8.2"", ""M20.01,46.32c1.61,0.8,2.86,0.43,4.86,0.03c5.87-1.17,15.18-2.91,16.08-3.04c2.3-0.32,2.98,1.17,2.77,2.63c-0.71,4.9-2.07,13.76-2.78,18.54c-0.27,1.86-0.45,3.1-0.45,3.26"", ""M22.71,57.53c4.17-0.91,13.42-2.41,18.53-3.16"", ""M22.37,68.85c5.51-0.98,11.63-1.73,17.21-2.47"", ""M20.37,47.01c0.69,0.69,1.03,1.8,1.03,2.74c0,5.75-0.29,36.68-0.33,38.97C21,92.75,21.5,93.75,26,90.5c2.1-1.52,7-5,12.5-8.25"", ""M37.25,74.25c1.59,2.03,4.09,7.46,5,11.75"", ""M61.5,14.25c0.25,1.12,0.42,2.14,0.18,3.31C60,25.75,55.82,40.12,49,49.5"", ""M58.22,37.74c1.9,0.39,3.14,0.18,4.51-0.14c1.67-0.39,21.21-5.25,22.76-5.6c6.75-1.5,3.5,4.5-3,11.25"", ""M64.85,47.5c0.78,1.5,0.86,2.88,0.62,4.52C63,68.62,57.25,84.5,42.25,95.75"", ""M65.42,60.5c3.56,6.77,15.26,21.56,22.62,29.46c1.89,2.03,3.98,4.66,6.71,5.54""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_098f2"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:098f2"" kvg:element=""飲"">
	<g id=""kvg:098f2-g1"" kvg:element=""飠"" kvg:original=""食"" kvg:partial=""true"" kvg:position=""left"" kvg:radical=""general"" kvg:phon=""食"">
		<path id=""kvg:098f2-s1"" kvg:type=""㇒"" d=""M31.53,14.5c0.06,0.73,0.24,1.94-0.12,2.92c-2.96,8.1-11.79,20.97-21.04,27.95""/>
		<path id=""kvg:098f2-s2"" kvg:type=""㇔/㇏"" d=""M33.5,19.5c5.48,2.15,11.9,6.21,15,10.75""/>
		<path id=""kvg:098f2-s3"" kvg:type=""㇑a"" d=""M32.79,32.26c0.85,0.85,1.28,2.38,1.28,3.32c0,3.54-0.08,5.92-0.08,8.2""/>
		<path id=""kvg:098f2-s4"" kvg:type=""㇕"" d=""M20.01,46.32c1.61,0.8,2.86,0.43,4.86,0.03c5.87-1.17,15.18-2.91,16.08-3.04c2.3-0.32,2.98,1.17,2.77,2.63c-0.71,4.9-2.07,13.76-2.78,18.54c-0.27,1.86-0.45,3.1-0.45,3.26""/>
		<path id=""kvg:098f2-s5"" kvg:type=""㇐a"" d=""M22.71,57.53c4.17-0.91,13.42-2.41,18.53-3.16""/>
		<path id=""kvg:098f2-s6"" kvg:type=""㇐a"" d=""M22.37,68.85c5.51-0.98,11.63-1.73,17.21-2.47""/>
		<path id=""kvg:098f2-s7"" kvg:type=""㇙"" d=""M20.37,47.01c0.69,0.69,1.03,1.8,1.03,2.74c0,5.75-0.29,36.68-0.33,38.97C21,92.75,21.5,93.75,26,90.5c2.1-1.52,7-5,12.5-8.25""/>
		<path id=""kvg:098f2-s8"" kvg:type=""㇔/㇏"" d=""M37.25,74.25c1.59,2.03,4.09,7.46,5,11.75""/>
	</g>
	<g id=""kvg:098f2-g2"" kvg:element=""欠"" kvg:position=""right"">
		<path id=""kvg:098f2-s9"" kvg:type=""㇒"" d=""M61.5,14.25c0.25,1.12,0.42,2.14,0.18,3.31C60,25.75,55.82,40.12,49,49.5""/>
		<path id=""kvg:098f2-s10"" kvg:type=""㇖a"" d=""M58.22,37.74c1.9,0.39,3.14,0.18,4.51-0.14c1.67-0.39,21.21-5.25,22.76-5.6c6.75-1.5,3.5,4.5-3,11.25""/>
		<path id=""kvg:098f2-s11"" kvg:type=""㇒"" d=""M64.85,47.5c0.78,1.5,0.86,2.88,0.62,4.52C63,68.62,57.25,84.5,42.25,95.75""/>
		<path id=""kvg:098f2-s12"" kvg:type=""㇏"" d=""M65.42,60.5c3.56,6.77,15.26,21.56,22.62,29.46c1.89,2.03,3.98,4.66,6.71,5.54""/>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_098f2"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 23.25 16.63)"">1</text>
	<text transform=""matrix(1 0 0 1 41.25 19.63)"">2</text>
	<text transform=""matrix(1 0 0 1 31.50 29.50)"">3</text>
	<text transform=""matrix(1 0 0 1 23.25 43.05)"">4</text>
	<text transform=""matrix(1 0 0 1 25.50 54.13)"">5</text>
	<text transform=""matrix(1 0 0 1 25.50 66.13)"">6</text>
	<text transform=""matrix(1 0 0 1 14.50 55.50)"">7</text>
	<text transform=""matrix(1 0 0 1 30.50 77.50)"">8</text>
	<text transform=""matrix(1 0 0 1 52.50 14.50)"">9</text>
	<text transform=""matrix(1 0 0 1 62.50 34.50)"">10</text>
	<text transform=""matrix(1 0 0 1 54.50 53.50)"">11</text>
	<text transform=""matrix(1 0 0 1 69.50 64.50)"">12</text>
</g>
</svg>
",
                OrderIndex = 8,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("c57d7cdf-eb30-5672-a5a4-2f911246487d"),
                LessonId = lesson5Id,
                Level = "N3",
                Character = "買",
                HanViet = "MÃI",
                Meaning = "Mua",
                StrokeCount = 12,
                KunReading = "かう",
                OnReading = "バイ",
                Mnemonic = "Giăng tấm lưới (罒) lớn gom thật nhiều tiền (貝) để đi mua sắm.",
                ComponentMapJson = @"[{""character"": ""罒"", ""component"": ""罒"", ""name"": ""罒"", ""meaning"": ""罒""}, {""character"": ""貝"", ""component"": ""貝"", ""name"": ""貝"", ""meaning"": ""貝""}]",
                StrokeDataJson = @"[""M20.5,14.64c0.49,0.49,1.44,1.59,1.66,2.43c1.09,4.17,1.51,6.88,2.4,11.91c0.2,1.15,0.39,2.26,0.55,3.26"", ""M23.25,16.2c15.2-1.38,58.15-3.72,64.01-4.22c2.74-0.23,4.12,1.39,3.32,3.95c-1.08,3.44-1.82,5.56-3.62,10.4c-0.38,1.02-0.87,1.99-1.32,2.94"", ""M43.25,16.5c0.75,0.75,0.68,1.25,0.85,2.27c0.6,3.64,1.21,7.47,1.4,9.23"", ""M66.25,14.5c0.5,0.75,0.42,2.03,0.29,3.01c-0.54,4.37-0.79,6.62-1.29,9.24"", ""M26.3,30.62c8.83-0.74,51.2-3.49,59.17-3.65"", ""M34.7,38.87c1.09,1.09,1.57,2.82,1.57,4.01c0,1.2,0.07,32.68,0.07,33.28s-0.07,2.47-0.07,3.8"", ""M37.02,40.77c8.15-0.63,32.05-3.03,34.84-3.03c2.89,0,4.19,1.26,4.19,3.7c0,2.44-0.08,19.9-0.06,33.08c0,1.35,0.04,2.86,0.04,4.34"", ""M37.53,52.38c5.97-0.26,31.44-2.43,37.25-2.43"", ""M37.5,64.75C46.25,64,65,62.5,74.59,62.1"", ""M37.76,76.86c8.24-0.48,27.49-1.73,36.87-2.04"", ""M43.77,83c0.23,1.5-0.38,2.82-1.5,3.83C38.75,90,30.5,95.5,21.5,99"", ""M69.84,83.97C75,87.91,80.38,94.38,82.5,98.5""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_08cb7"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:08cb7"" kvg:element=""買"">
	<g id=""kvg:08cb7-g1"" kvg:element=""罒"" kvg:variant=""true"" kvg:original=""网"" kvg:position=""top"" kvg:radical=""nelson"">
		<path id=""kvg:08cb7-s1"" kvg:type=""㇑"" d=""M20.5,14.64c0.49,0.49,1.44,1.59,1.66,2.43c1.09,4.17,1.51,6.88,2.4,11.91c0.2,1.15,0.39,2.26,0.55,3.26""/>
		<path id=""kvg:08cb7-s2"" kvg:type=""㇕a"" d=""M23.25,16.2c15.2-1.38,58.15-3.72,64.01-4.22c2.74-0.23,4.12,1.39,3.32,3.95c-1.08,3.44-1.82,5.56-3.62,10.4c-0.38,1.02-0.87,1.99-1.32,2.94""/>
		<path id=""kvg:08cb7-s3"" kvg:type=""㇑a"" d=""M43.25,16.5c0.75,0.75,0.68,1.25,0.85,2.27c0.6,3.64,1.21,7.47,1.4,9.23""/>
		<path id=""kvg:08cb7-s4"" kvg:type=""㇑a"" d=""M66.25,14.5c0.5,0.75,0.42,2.03,0.29,3.01c-0.54,4.37-0.79,6.62-1.29,9.24""/>
		<path id=""kvg:08cb7-s5"" kvg:type=""㇐a"" d=""M26.3,30.62c8.83-0.74,51.2-3.49,59.17-3.65""/>
	</g>
	<g id=""kvg:08cb7-g2"" kvg:element=""貝"" kvg:position=""bottom"" kvg:radical=""tradit"">
		<g id=""kvg:08cb7-g3"" kvg:element=""目"" kvg:position=""top"">
			<path id=""kvg:08cb7-s6"" kvg:type=""㇑"" d=""M34.7,38.87c1.09,1.09,1.57,2.82,1.57,4.01c0,1.2,0.07,32.68,0.07,33.28s-0.07,2.47-0.07,3.8""/>
			<path id=""kvg:08cb7-s7"" kvg:type=""㇕a"" d=""M37.02,40.77c8.15-0.63,32.05-3.03,34.84-3.03c2.89,0,4.19,1.26,4.19,3.7c0,2.44-0.08,19.9-0.06,33.08c0,1.35,0.04,2.86,0.04,4.34""/>
			<path id=""kvg:08cb7-s8"" kvg:type=""㇐a"" d=""M37.53,52.38c5.97-0.26,31.44-2.43,37.25-2.43""/>
			<path id=""kvg:08cb7-s9"" kvg:type=""㇐a"" d=""M37.5,64.75C46.25,64,65,62.5,74.59,62.1""/>
			<path id=""kvg:08cb7-s10"" kvg:type=""㇐a"" d=""M37.76,76.86c8.24-0.48,27.49-1.73,36.87-2.04""/>
		</g>
		<g id=""kvg:08cb7-g4"" kvg:element=""八"" kvg:position=""bottom"">
			<path id=""kvg:08cb7-s11"" kvg:type=""㇒"" d=""M43.77,83c0.23,1.5-0.38,2.82-1.5,3.83C38.75,90,30.5,95.5,21.5,99""/>
			<path id=""kvg:08cb7-s12"" kvg:type=""㇔"" d=""M69.84,83.97C75,87.91,80.38,94.38,82.5,98.5""/>
		</g>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_08cb7"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 14.50 22.50)"">1</text>
	<text transform=""matrix(1 0 0 1 24.50 13.50)"">2</text>
	<text transform=""matrix(1 0 0 1 37.50 24.50)"">3</text>
	<text transform=""matrix(1 0 0 1 59.50 22.50)"">4</text>
	<text transform=""matrix(1 0 0 1 27.50 27.50)"">5</text>
	<text transform=""matrix(1 0 0 1 28.50 47.50)"">6</text>
	<text transform=""matrix(1 0 0 1 38.50 39.13)"">7</text>
	<text transform=""matrix(1 0 0 1 39.50 49.63)"">8</text>
	<text transform=""matrix(1 0 0 1 39.50 61.63)"">9</text>
	<text transform=""matrix(1 0 0 1 39.50 73.63)"">10</text>
	<text transform=""matrix(1 0 0 1 28.50 88.63)"">11</text>
	<text transform=""matrix(1 0 0 1 57.50 87.50)"">12</text>
</g>
</svg>
",
                OrderIndex = 9,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("befa0080-7734-5a82-831c-7e2a56ef604e"),
                LessonId = lesson5Id,
                Level = "N3",
                Character = "物",
                HanViet = "VẬT",
                Meaning = "Đồ vật",
                StrokeCount = 8,
                KunReading = "もの",
                OnReading = "ブツ、モツ",
                Mnemonic = "Xin đừng (勿) đối xử với con bò (牛) như một món đồ vật vô tri.",
                ComponentMapJson = @"[{""character"": ""勿"", ""component"": ""勿"", ""name"": ""勿"", ""meaning"": ""勿""}, {""character"": ""牛"", ""component"": ""牛"", ""name"": ""牛"", ""meaning"": ""牛""}]",
                StrokeDataJson = @"[""M24.27,24.64c0.03,0.65,0.07,1.68-0.06,2.61c-0.78,5.5-5.26,17.57-11.4,24.96"", ""M24.25,38.75c1,0.12,2.08-0.01,3.24-0.19c6.4-1,11.5-2.03,15.64-2.93c1.75-0.38,3.57-0.44,5.12-0.13"", ""M35.37,13.75c1.19,1.19,1.46,2.88,1.46,4c0,0.88-0.16,52.04-0.21,71.5c-0.01,3.29-0.01,5.67-0.01,6.75"", ""M14.83,70.21c1.28,0.62,2.49,0.68,3.86-0.21C19.84,69.27,39.15,56.56,44,53"", ""M67.27,15.75c0.05,0.66,0.2,1.74-0.1,2.66c-2.36,7.42-7.99,16.52-17.29,25.46"", ""M59.25,38.66c1.75,0.47,3.07,0.46,5.16,0.17c6.96-0.96,18.71-3.46,25.66-4.75c5.08-0.95,6.2,2.13,5.42,6.59c-0.77,4.47-5.95,37.3-17.3,49.42c-3.41,3.64-5.21,1.6-8.26-1.06"", ""M66.26,41.88c0.06,0.64,0.07,1.99-0.36,2.83c-4.54,8.79-9.7,16.04-20.02,24.7"", ""M79.62,39.6c0.51,1.27,0.41,2.72-0.16,4.11c-4.52,11.13-11.71,24.8-27.21,39.28""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_07269"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:07269"" kvg:element=""物"">
	<g id=""kvg:07269-g1"" kvg:element=""牛"" kvg:variant=""true"" kvg:position=""left"" kvg:radical=""general"">
		<path id=""kvg:07269-s1"" kvg:type=""㇒"" d=""M24.27,24.64c0.03,0.65,0.07,1.68-0.06,2.61c-0.78,5.5-5.26,17.57-11.4,24.96""/>
		<path id=""kvg:07269-s2"" kvg:type=""㇐"" d=""M24.25,38.75c1,0.12,2.08-0.01,3.24-0.19c6.4-1,11.5-2.03,15.64-2.93c1.75-0.38,3.57-0.44,5.12-0.13""/>
		<path id=""kvg:07269-s3"" kvg:type=""㇑"" d=""M35.37,13.75c1.19,1.19,1.46,2.88,1.46,4c0,0.88-0.16,52.04-0.21,71.5c-0.01,3.29-0.01,5.67-0.01,6.75""/>
		<path id=""kvg:07269-s4"" kvg:type=""㇀"" d=""M14.83,70.21c1.28,0.62,2.49,0.68,3.86-0.21C19.84,69.27,39.15,56.56,44,53""/>
	</g>
	<g id=""kvg:07269-g2"" kvg:element=""勿"" kvg:position=""right"" kvg:phon=""勿"">
		<g id=""kvg:07269-g3"" kvg:element=""勹"">
			<path id=""kvg:07269-s5"" kvg:type=""㇒"" d=""M67.27,15.75c0.05,0.66,0.2,1.74-0.1,2.66c-2.36,7.42-7.99,16.52-17.29,25.46""/>
			<path id=""kvg:07269-s6"" kvg:type=""㇆"" d=""M59.25,38.66c1.75,0.47,3.07,0.46,5.16,0.17c6.96-0.96,18.71-3.46,25.66-4.75c5.08-0.95,6.2,2.13,5.42,6.59c-0.77,4.47-5.95,37.3-17.3,49.42c-3.41,3.64-5.21,1.6-8.26-1.06""/>
		</g>
		<path id=""kvg:07269-s7"" kvg:type=""㇒"" d=""M66.26,41.88c0.06,0.64,0.07,1.99-0.36,2.83c-4.54,8.79-9.7,16.04-20.02,24.7""/>
		<path id=""kvg:07269-s8"" kvg:type=""㇒"" d=""M79.62,39.6c0.51,1.27,0.41,2.72-0.16,4.11c-4.52,11.13-11.71,24.8-27.21,39.28""/>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_07269"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 16.50 24.50)"">1</text>
	<text transform=""matrix(1 0 0 1 28.50 35.50)"">2</text>
	<text transform=""matrix(1 0 0 1 26.50 14.50)"">3</text>
	<text transform=""matrix(1 0 0 1 7.75 75.13)"">4</text>
	<text transform=""matrix(1 0 0 1 57.50 15.50)"">5</text>
	<text transform=""matrix(1 0 0 1 65.25 35.50)"">6</text>
	<text transform=""matrix(1 0 0 1 56.25 49.63)"">7</text>
	<text transform=""matrix(1 0 0 1 70.50 49.50)"">8</text>
</g>
</svg>
",
                OrderIndex = 10,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("312ced8d-b411-57d0-9d7e-37aeefa5f63c"),
                LessonId = lesson5Id,
                Level = "N3",
                Character = "行",
                HanViet = "HÀNH",
                Meaning = "Đi, thực hiện",
                StrokeCount = 6,
                KunReading = "いく、ゆく、おこなう",
                OnReading = "コウ、ギョウ",
                Mnemonic = "Bước chân trái (彳) và bước chân phải (亍) cùng phối hợp tiến lên để đi.",
                ComponentMapJson = @"[{""character"": ""彳"", ""component"": ""彳"", ""name"": ""彳"", ""meaning"": ""彳""}, {""character"": ""亍"", ""component"": ""亍"", ""name"": ""亍"", ""meaning"": ""亍""}]",
                StrokeDataJson = @"[""M32.49,12c-0.12,1-0.45,1.9-1.1,2.62C28.29,18.06,22.2,22.6,12.5,28"", ""M36.5,31.75c0.07,0.73,0.08,2.28-0.39,3.18C32.12,42.5,23.83,52.5,11,62.75"", ""M25.57,51.75c0.9,0.9,1.23,2.25,1.23,3.26c0,0.72,0.04,24.47-0.07,35.49c-0.02,2.19-0.04,3.87-0.07,4.75"", ""M50.5,18.45c1.44,0.35,3.81,0.52,5.23,0.35c7.14-0.8,16.01-2.43,24.49-3.06c2.38-0.18,3.83-0.06,5.02,0.11"", ""M43.13,41.42c1.5,0.38,4.27,0.58,5.76,0.38c12.86-1.67,28.86-4.05,41.85-5.38c2.49-0.26,4.01,0.18,5.26,0.37"", ""M71.52,41.33c1.26,1.26,1.76,2.79,1.76,5.27c0,14.56-0.26,38.66-0.26,43.62c0,8.03-7.21-0.5-8.71-1.75""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_0884c"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:0884c"" kvg:element=""行"" kvg:radical=""general"">
	<g id=""kvg:0884c-g1"" kvg:element=""彳"" kvg:position=""left"">
		<path id=""kvg:0884c-s1"" kvg:type=""㇒"" d=""M32.49,12c-0.12,1-0.45,1.9-1.1,2.62C28.29,18.06,22.2,22.6,12.5,28""/>
		<g id=""kvg:0884c-g2"" kvg:element=""亻"" kvg:variant=""true"" kvg:original=""人"">
			<path id=""kvg:0884c-s2"" kvg:type=""㇒"" d=""M36.5,31.75c0.07,0.73,0.08,2.28-0.39,3.18C32.12,42.5,23.83,52.5,11,62.75""/>
			<path id=""kvg:0884c-s3"" kvg:type=""㇑"" d=""M25.57,51.75c0.9,0.9,1.23,2.25,1.23,3.26c0,0.72,0.04,24.47-0.07,35.49c-0.02,2.19-0.04,3.87-0.07,4.75""/>
		</g>
	</g>
	<g id=""kvg:0884c-g3"" kvg:position=""right"">
		<path id=""kvg:0884c-s4"" kvg:type=""㇐"" d=""M50.5,18.45c1.44,0.35,3.81,0.52,5.23,0.35c7.14-0.8,16.01-2.43,24.49-3.06c2.38-0.18,3.83-0.06,5.02,0.11""/>
		<path id=""kvg:0884c-s5"" kvg:type=""㇐"" d=""M43.13,41.42c1.5,0.38,4.27,0.58,5.76,0.38c12.86-1.67,28.86-4.05,41.85-5.38c2.49-0.26,4.01,0.18,5.26,0.37""/>
		<path id=""kvg:0884c-s6"" kvg:type=""㇚"" d=""M71.52,41.33c1.26,1.26,1.76,2.79,1.76,5.27c0,14.56-0.26,38.66-0.26,43.62c0,8.03-7.21-0.5-8.71-1.75""/>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_0884c"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 25.75 10.50)"">1</text>
	<text transform=""matrix(1 0 0 1 28.50 31.50)"">2</text>
	<text transform=""matrix(1 0 0 1 19.50 65.50)"">3</text>
	<text transform=""matrix(1 0 0 1 49.50 15.50)"">4</text>
	<text transform=""matrix(1 0 0 1 43.50 38.50)"">5</text>
	<text transform=""matrix(1 0 0 1 64.50 50.50)"">6</text>
</g>
</svg>
",
                OrderIndex = 11,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("f50b90fa-dbd3-5630-9e71-662bc64e0a5d"),
                LessonId = lesson5Id,
                Level = "N3",
                Character = "休",
                HanViet = "HƯU",
                Meaning = "Nghỉ ngơi",
                StrokeCount = 6,
                KunReading = "やすむ",
                OnReading = "キュウ",
                Mnemonic = "Người (亻) tựa lưng vào gốc cây (木) để nghỉ ngơi.",
                ComponentMapJson = @"[{""character"": ""亻"", ""component"": ""亻"", ""name"": ""亻"", ""meaning"": ""亻""}, {""character"": ""木"", ""component"": ""木"", ""name"": ""木"", ""meaning"": ""木""}]",
                StrokeDataJson = @"[""M35,16.5c0.25,1.75,0.25,4.25-0.88,6.8C28.91,35.01,22.37,46.02,10.5,60.29"", ""M26.28,42.5c0.72,1.25,1.26,3.48,1.26,4.75c0,12.75-0.07,29.88-0.26,42.25c-0.02,1.54-0.04,2.97-0.04,4.25"", ""M37.65,38.83c2.45,0.97,5.18,0.75,7.73,0.54c11.76-0.97,24.94-3.35,37.49-4.01c2.65-0.14,5.39-0.22,7.99,0.39"", ""M61.43,14c0.82,0.75,1.87,2.12,1.87,3.7c0,8.8,0.05,53.72-0.12,72.05c-0.03,2.88-0.06,4.91-0.08,5.75"", ""M62.43,38.32c0,2.18-1.1,4.31-1.9,6.04C54.57,57.4,44.96,71.84,35,78.75"", ""M64.12,38.08c4.45,8.37,16.21,25.33,24.99,33.19c1.96,1.76,4.35,4.18,6.9,5""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_04f11"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:04f11"" kvg:element=""休"">
	<g id=""kvg:04f11-g1"" kvg:element=""亻"" kvg:variant=""true"" kvg:original=""人"" kvg:position=""left"" kvg:radical=""general"">
		<path id=""kvg:04f11-s1"" kvg:type=""㇒"" d=""M35,16.5c0.25,1.75,0.25,4.25-0.88,6.8C28.91,35.01,22.37,46.02,10.5,60.29""/>
		<path id=""kvg:04f11-s2"" kvg:type=""㇑"" d=""M26.28,42.5c0.72,1.25,1.26,3.48,1.26,4.75c0,12.75-0.07,29.88-0.26,42.25c-0.02,1.54-0.04,2.97-0.04,4.25""/>
	</g>
	<g id=""kvg:04f11-g2"" kvg:element=""木"" kvg:position=""right"">
		<path id=""kvg:04f11-s3"" kvg:type=""㇐"" d=""M37.65,38.83c2.45,0.97,5.18,0.75,7.73,0.54c11.76-0.97,24.94-3.35,37.49-4.01c2.65-0.14,5.39-0.22,7.99,0.39""/>
		<path id=""kvg:04f11-s4"" kvg:type=""㇑"" d=""M61.43,14c0.82,0.75,1.87,2.12,1.87,3.7c0,8.8,0.05,53.72-0.12,72.05c-0.03,2.88-0.06,4.91-0.08,5.75""/>
		<path id=""kvg:04f11-s5"" kvg:type=""㇒"" d=""M62.43,38.32c0,2.18-1.1,4.31-1.9,6.04C54.57,57.4,44.96,71.84,35,78.75""/>
		<path id=""kvg:04f11-s6"" kvg:type=""㇏"" d=""M64.12,38.08c4.45,8.37,16.21,25.33,24.99,33.19c1.96,1.76,4.35,4.18,6.9,5""/>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_04f11"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 25.50 15.50)"">1</text>
	<text transform=""matrix(1 0 0 1 20.50 58.50)"">2</text>
	<text transform=""matrix(1 0 0 1 38.50 36.50)"">3</text>
	<text transform=""matrix(1 0 0 1 52.50 14.50)"">4</text>
	<text transform=""matrix(1 0 0 1 50.50 49.50)"">5</text>
	<text transform=""matrix(1 0 0 1 73.50 48.50)"">6</text>
</g>
</svg>
",
                OrderIndex = 12,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
        };

        foreach (var item in kanjiItems5)
        {
            if (!await db.KanjiItems.AnyAsync(k => k.Character == item.Character))
                db.KanjiItems.Add(item);
        }

        var vocabItems5 = new List<KanjiVocabulary>
        {
            new KanjiVocabulary {
                Id = Guid.Parse("ebcc7cb9-2fbf-52f3-9fac-d82ed44d7615"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("f8571dc2-b128-582f-afe8-56183685f8d5"),
                Level = "N3",
                Word = "先生",
                Reading = "せんせい",
                Meaning = "thầy/cô giáo",
                OrderIndex = 1,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("6704da3c-ef4f-5191-88e4-7c51ff538d86"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("f8571dc2-b128-582f-afe8-56183685f8d5"),
                Level = "N3",
                Word = "先日",
                Reading = "せんじつ",
                Meaning = "vài ngày trước",
                OrderIndex = 2,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("e49ca78f-4fc6-5f2e-a4ec-b9f8564af74d"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("f8571dc2-b128-582f-afe8-56183685f8d5"),
                Level = "N3",
                Word = "先月",
                Reading = "せんげつ",
                Meaning = "tháng trước",
                OrderIndex = 3,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("bbe3845e-362c-5fa7-869b-129f1bcbbb05"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("f8571dc2-b128-582f-afe8-56183685f8d5"),
                Level = "N3",
                Word = "先週",
                Reading = "せんしゅう",
                Meaning = "tuần trước",
                OrderIndex = 4,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("81e8cd2c-2c30-5b6b-bae6-c0ee4de6d138"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("1d97f9ed-5faa-57cf-bf8b-3adb806a8f0b"),
                Level = "N3",
                Word = "一週間",
                Reading = "いっしゅうかん",
                Meaning = "1 tuần",
                OrderIndex = 5,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("f6269366-7e45-52e9-88e5-de8bab4cdf26"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("f719f4c9-db44-595f-8a75-f3530018ae3d"),
                Level = "N3",
                Word = "毎日",
                Reading = "まいにち",
                Meaning = "mỗi ngày",
                OrderIndex = 6,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("6ec29966-a8ce-5d88-9594-472ecee97d0f"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("1d97f9ed-5faa-57cf-bf8b-3adb806a8f0b"),
                Level = "N3",
                Word = "毎週",
                Reading = "まいしゅう",
                Meaning = "mỗi tuần",
                OrderIndex = 7,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("ed8fe2c4-5afd-53e2-a68b-93a0154e33e4"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("f719f4c9-db44-595f-8a75-f3530018ae3d"),
                Level = "N3",
                Word = "毎年",
                Reading = "まいとし/まいねん",
                Meaning = "mỗi năm",
                OrderIndex = 8,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("7ac3b330-0316-58d8-a473-dba1c3a80ceb"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("f719f4c9-db44-595f-8a75-f3530018ae3d"),
                Level = "N3",
                Word = "毎月",
                Reading = "まいつき",
                Meaning = "mỗi tháng",
                OrderIndex = 9,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("8dce2a1a-28c2-5cff-a946-1d2843f216e6"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("95b380a4-8169-5d38-b87a-bca35a0ac341"),
                Level = "N3",
                Word = "後ろ",
                Reading = "うしろ",
                Meaning = "phía sau",
                OrderIndex = 10,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("ffae1439-e319-54bf-b2e3-322c330e25b8"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("95b380a4-8169-5d38-b87a-bca35a0ac341"),
                Level = "N3",
                Word = "後",
                Reading = "あと",
                Meaning = "sau đó",
                OrderIndex = 11,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("bded48d1-de4e-589e-a5d1-70a983b9b0c8"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("b1762e10-ea10-50cb-9513-ce389737a925"),
                Level = "N3",
                Word = "午後",
                Reading = "ごご",
                Meaning = "p.m",
                OrderIndex = 12,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("8617f98f-fb68-5fe6-920c-0cf6dee21612"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("95b380a4-8169-5d38-b87a-bca35a0ac341"),
                Level = "N3",
                Word = "前後",
                Reading = "ぜんご",
                Meaning = "trước sau",
                OrderIndex = 13,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("01396178-b469-5fef-b00a-4faff5a9a6b1"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("a7b0cfdd-9764-55e2-a5f5-adf8d3f005f7"),
                Level = "N3",
                Word = "見ます",
                Reading = "みます",
                Meaning = "nhìn",
                OrderIndex = 14,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("4375f611-fe31-564c-952a-7f9cd31dd987"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("a7b0cfdd-9764-55e2-a5f5-adf8d3f005f7"),
                Level = "N3",
                Word = "見学",
                Reading = "けんがく",
                Meaning = "tham quan kiến tập",
                OrderIndex = 15,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("b0ca1a6d-b37f-5e92-b9a9-1d7474776ff9"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("d3679ee3-f082-5ca0-8920-cf1336840317"),
                Level = "N3",
                Word = "食べます",
                Reading = "たべます",
                Meaning = "ăn",
                OrderIndex = 16,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("7e8bcd97-9072-5d79-b395-a63a7d9d279a"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("d3679ee3-f082-5ca0-8920-cf1336840317"),
                Level = "N3",
                Word = "食事",
                Reading = "しょくじ",
                Meaning = "ăn, bữa ăn",
                OrderIndex = 17,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("895eda7a-c984-5c40-bf58-65a445339b14"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("a03537e6-2ed4-5467-b66a-dc384eaa4687"),
                Level = "N3",
                Word = "飲みます",
                Reading = "のみます",
                Meaning = "uống",
                OrderIndex = 18,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("9ef77259-c78e-547c-b8ac-416bee4cb2e1"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("d3679ee3-f082-5ca0-8920-cf1336840317"),
                Level = "N3",
                Word = "飲食",
                Reading = "いんしょく",
                Meaning = "ẩm thực",
                OrderIndex = 19,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("1eb1c03b-fd74-553b-bb04-a54bfa20f01b"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("a03537e6-2ed4-5467-b66a-dc384eaa4687"),
                Level = "N3",
                Word = "飲み水",
                Reading = "のみみず",
                Meaning = "nước uống",
                OrderIndex = 20,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("63cd79da-6145-597f-b52e-1bb3d9999737"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("f50b90fa-dbd3-5630-9e71-662bc64e0a5d"),
                Level = "N3",
                Word = "休みます",
                Reading = "やすみます",
                Meaning = "nghỉ",
                OrderIndex = 21,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("b01c3b1b-3f93-58f5-9f0f-46d2e040f27c"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("f50b90fa-dbd3-5630-9e71-662bc64e0a5d"),
                Level = "N3",
                Word = "休みの日",
                Reading = "やすみのひ",
                Meaning = "ngày nghỉ",
                OrderIndex = 22,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("0e3b4753-c54c-512e-8b20-a14f35af0b3e"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("f50b90fa-dbd3-5630-9e71-662bc64e0a5d"),
                Level = "N3",
                Word = "休日",
                Reading = "きゅうじつ",
                Meaning = "ngày nghỉ",
                OrderIndex = 23,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("8a2dfc71-b224-5d65-bf28-2644246b9702"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("c57d7cdf-eb30-5672-a5a4-2f911246487d"),
                Level = "N3",
                Word = "買います",
                Reading = "かいます",
                Meaning = "mua",
                OrderIndex = 24,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("23995647-53f8-5554-9345-fc499f0ee552"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("befa0080-7734-5a82-831c-7e2a56ef604e"),
                Level = "N3",
                Word = "物",
                Reading = "もの",
                Meaning = "đồ vật",
                OrderIndex = 25,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("91e3e106-d51b-5f04-9f8e-c42702bfce20"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("c57d7cdf-eb30-5672-a5a4-2f911246487d"),
                Level = "N3",
                Word = "買い物",
                Reading = "かいもの",
                Meaning = "mua sắm",
                OrderIndex = 26,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("3c097986-b70b-501c-a5e1-70c6b6eec967"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("d3679ee3-f082-5ca0-8920-cf1336840317"),
                Level = "N3",
                Word = "食べ物",
                Reading = "たべもの",
                Meaning = "đồ ăn",
                OrderIndex = 27,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("049782a3-32c7-5d80-be6a-bcc4a7695233"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("a03537e6-2ed4-5467-b66a-dc384eaa4687"),
                Level = "N3",
                Word = "飲み物",
                Reading = "のみもの",
                Meaning = "đồ uống",
                OrderIndex = 28,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("f98a1aa8-5a01-5cfc-9ac0-13b44e6fa0b3"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("befa0080-7734-5a82-831c-7e2a56ef604e"),
                Level = "N3",
                Word = "人物",
                Reading = "じんぶつ",
                Meaning = "nhân vật",
                OrderIndex = 29,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("aa379166-1812-5d7e-898c-37d45a9b46b7"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("a7b0cfdd-9764-55e2-a5f5-adf8d3f005f7"),
                Level = "N3",
                Word = "見物",
                Reading = "けんぶつ",
                Meaning = "tham quan",
                OrderIndex = 30,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("8233df81-2e23-599c-910f-5b33c75fe36e"),
                LessonId = lesson5Id,
                KanjiItemId = Guid.Parse("312ced8d-b411-57d0-9d7e-37aeefa5f63c"),
                Level = "N3",
                Word = "行きます",
                Reading = "いきます",
                Meaning = "đi",
                OrderIndex = 31,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
        };

        foreach (var item in vocabItems5)
        {
            if (!await db.KanjiVocabularyItems.AnyAsync(v => v.Word == item.Word && v.Reading == item.Reading))
                db.KanjiVocabularyItems.Add(item);
        }

        // Lesson 6: Giao tiếp và Sinh hoạt (JPD123)
        var lesson6Id = Guid.Parse("7e64f2ed-a054-568a-8109-16708122f2f3");
        if (!await db.KanjiLessons.AnyAsync(l => l.Id == lesson6Id))
        {
            db.KanjiLessons.Add(new KanjiLesson { Id = lesson6Id, Level = "N3", LessonNumber = 6, Title = "Giao tiếp và Sinh hoạt", Description = "Giao tiếp và Sinh hoạt - JPD123 Kanji N3.", AccessTier = "free", PackageCode = "kanji_jpd123", OrderIndex = 6, CreatedAt = SeededAt, UpdatedAt = SeededAt });
        }

        var kanjiItems6 = new List<KanjiItem>
        {
            new KanjiItem {
                Id = Guid.Parse("f0bb2995-c00a-5312-bf0c-dad7fdc550e7"),
                LessonId = lesson6Id,
                Level = "N3",
                Character = "今",
                HanViet = "KIM",
                Meaning = "Bây giờ, hiện tại",
                StrokeCount = 4,
                KunReading = "いま",
                OnReading = "コン、キン",
                Mnemonic = "Người (𠆢) vội vã chạy (ラ) về nhà ngay bây giờ (今).",
                ComponentMapJson = @"[]",
                StrokeDataJson = @"[""M49.42,14.25c0.1,1.11-0.11,2.93-0.71,4.47C44.5,29.5,32,47.25,11.5,61.75"", ""M50.66,18.99c6.1,7.28,32.37,31.03,39.1,36.36c2.28,1.81,5.21,2.58,7.49,3.09"", ""M39.23,50.26c1.27,0.24,2.64,0.37,4.13,0.18c5.39-0.68,11.02-1.69,15.86-2.31c1.8-0.23,3.66-0.38,4.8-0.08"", ""M33.25,67.75c2.12,0.38,3.57,0.61,6,0.25c6.31-0.93,18.5-3.25,25.24-4.44C68.48,62.85,70,65,68,68.75C63.33,77.5,58.75,85,53,94.5""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_04eca"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:04eca"" kvg:element=""今"">
	<g id=""kvg:04eca-g1"" kvg:element=""人"" kvg:position=""top"" kvg:radical=""general"">
		<path id=""kvg:04eca-s1"" kvg:type=""㇒"" d=""M49.42,14.25c0.1,1.11-0.11,2.93-0.71,4.47C44.5,29.5,32,47.25,11.5,61.75""/>
		<path id=""kvg:04eca-s2"" kvg:type=""㇏"" d=""M50.66,18.99c6.1,7.28,32.37,31.03,39.1,36.36c2.28,1.81,5.21,2.58,7.49,3.09""/>
	</g>
	<g id=""kvg:04eca-g2"" kvg:position=""bottom"">
		<g id=""kvg:04eca-g3"" kvg:element=""一"">
			<path id=""kvg:04eca-s3"" kvg:type=""㇐"" d=""M39.23,50.26c1.27,0.24,2.64,0.37,4.13,0.18c5.39-0.68,11.02-1.69,15.86-2.31c1.8-0.23,3.66-0.38,4.8-0.08""/>
		</g>
		<path id=""kvg:04eca-s4"" kvg:type=""㇇"" d=""M33.25,67.75c2.12,0.38,3.57,0.61,6,0.25c6.31-0.93,18.5-3.25,25.24-4.44C68.48,62.85,70,65,68,68.75C63.33,77.5,58.75,85,53,94.5""/>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_04eca"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 41.50 13.50)"">1</text>
	<text transform=""matrix(1 0 0 1 58.50 22.63)"">2</text>
	<text transform=""matrix(1 0 0 1 38.50 47.50)"">3</text>
	<text transform=""matrix(1 0 0 1 25.50 69.50)"">4</text>
</g>
</svg>
",
                OrderIndex = 1,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("e2ddd4ff-b156-54da-b717-94b9ea0625bc"),
                LessonId = lesson6Id,
                Level = "N3",
                Character = "来",
                HanViet = "LAI",
                Meaning = "Đến",
                StrokeCount = 7,
                KunReading = "く.る、き.たる",
                OnReading = "ライ",
                Mnemonic = "Mang theo hai (丷) hạt giống đến (来) gộp thành một (一) đống dưới gốc cây (木).",
                ComponentMapJson = @"[{""character"": ""丷"", ""component"": ""丷"", ""name"": ""丷"", ""meaning"": ""丷""}, {""character"": ""一"", ""component"": ""一"", ""name"": ""一"", ""meaning"": ""一""}, {""character"": ""木"", ""component"": ""木"", ""name"": ""木"", ""meaning"": ""木""}]",
                StrokeDataJson = @"[""M25.54,28.33c1.61,0.39,4.58,0.53,6.19,0.39c16.32-1.46,27.01-3.46,43.69-3.67c2.69-0.03,4.31,0.18,5.65,0.38"", ""M30.12,37.62c2.85,2.07,7.16,7.91,7.88,11.12"", ""M74.52,33c0.08,0.98-0.11,1.9-0.58,2.77c-1.33,3.04-4.7,7.77-9.06,10.86"", ""M16.62,57c2.28,0.5,4.9,0.74,8.42,0.5c14.81-1,39.08-3.5,58.03-4.25c3.54-0.14,6.33,0.25,8.55,0.5"", ""M51.67,10.75c1.33,1,2.18,2.75,2.18,4.5c0,0.9,0.06,58.96-0.17,78c-0.03,2.77-0.07,4.71-0.1,5.5"", ""M49.75,56.5c0,1.5-0.44,2.48-0.82,3.11C42.37,70.49,29,83.75,15.75,90.5"", ""M55,56.25c4.38,3.88,19.75,19,29.73,26.28c2.82,2.06,6.52,4.5,10.02,5.22""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_06765"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:06765"" kvg:element=""来"">
	<path id=""kvg:06765-s1"" kvg:type=""㇐"" d=""M25.54,28.33c1.61,0.39,4.58,0.53,6.19,0.39c16.32-1.46,27.01-3.46,43.69-3.67c2.69-0.03,4.31,0.18,5.65,0.38""/>
	<g id=""kvg:06765-g1"" kvg:element=""米"">
		<path id=""kvg:06765-s2"" kvg:type=""㇔"" d=""M30.12,37.62c2.85,2.07,7.16,7.91,7.88,11.12""/>
		<path id=""kvg:06765-s3"" kvg:type=""㇒"" d=""M74.52,33c0.08,0.98-0.11,1.9-0.58,2.77c-1.33,3.04-4.7,7.77-9.06,10.86""/>
		<g id=""kvg:06765-g2"" kvg:element=""木"" kvg:radical=""tradit"">
			<path id=""kvg:06765-s4"" kvg:type=""㇐"" d=""M16.62,57c2.28,0.5,4.9,0.74,8.42,0.5c14.81-1,39.08-3.5,58.03-4.25c3.54-0.14,6.33,0.25,8.55,0.5""/>
			<path id=""kvg:06765-s5"" kvg:type=""㇑"" d=""M51.67,10.75c1.33,1,2.18,2.75,2.18,4.5c0,0.9,0.06,58.96-0.17,78c-0.03,2.77-0.07,4.71-0.1,5.5""/>
			<g id=""kvg:06765-g3"" kvg:element=""丿"" kvg:radical=""nelson"">
				<path id=""kvg:06765-s6"" kvg:type=""㇒"" d=""M49.75,56.5c0,1.5-0.44,2.48-0.82,3.11C42.37,70.49,29,83.75,15.75,90.5""/>
			</g>
			<path id=""kvg:06765-s7"" kvg:type=""㇏"" d=""M55,56.25c4.38,3.88,19.75,19,29.73,26.28c2.82,2.06,6.52,4.5,10.02,5.22""/>
		</g>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_06765"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 18.50 28.50)"">1</text>
	<text transform=""matrix(1 0 0 1 23.50 40.63)"">2</text>
	<text transform=""matrix(1 0 0 1 65.50 35.50)"">3</text>
	<text transform=""matrix(1 0 0 1 9.50 58.50)"">4</text>
	<text transform=""matrix(1 0 0 1 42.50 13.63)"">5</text>
	<text transform=""matrix(1 0 0 1 35.50 67.50)"">6</text>
	<text transform=""matrix(1 0 0 1 70.50 66.50)"">7</text>
</g>
</svg>
",
                OrderIndex = 2,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("ccd5f9fb-158b-58ea-9717-1806bf5c0d89"),
                LessonId = lesson6Id,
                Level = "N3",
                Character = "帰",
                HanViet = "QUY",
                Meaning = "Trở về",
                StrokeCount = 10,
                KunReading = "かえ.る",
                OnReading = "キ",
                Mnemonic = "Cầm đao (刂) quét chổi (ヨ) dọn dẹp, sau đó trùm khăn (冖) che đồ lại để trở về (帰) nhà.",
                ComponentMapJson = @"[{""character"": ""刂"", ""component"": ""刂"", ""name"": ""刂"", ""meaning"": ""刂""}, {""character"": ""冖"", ""component"": ""冖"", ""name"": ""冖"", ""meaning"": ""冖""}]",
                StrokeDataJson = @"[""M16.46,35.12c0.71,0.71,1.26,1.65,1.26,2.99c0,5.61-0.08,13.76-0.12,19.77c-0.01,1.77-0.02,3.35-0.02,4.63"", ""M26.46,17.62c0.84,0.84,1.23,2.1,1.26,3.74c0.66,43.52-0.21,51.27-10.21,62.89"", ""M45.75,15.5c1.01,0.24,2.51,0.63,4.18,0.43C57.5,15,71.38,13.38,78,12.25c1.52-0.26,3.42,0.75,3.05,2.44c-1.03,4.73-2.29,13.19-3.06,18.34c-0.22,1.49-0.41,2.74-0.54,3.62"", ""M44.89,26.32c1.47,0.38,3.01,0.52,4.49,0.38c10.59-1,21.15-1.96,29.47-2.4"", ""M43.68,37.85c1.65,0.5,3.41,0.49,5.08,0.27c8.74-1.13,19.84-2.66,27.98-3.19"", ""M40.88,48.75c-0.09,3.63-2.32,9.83-3.12,12"", ""M41.49,51.15c11.51-1.9,35.42-5.38,45.63-5.93C96,44.75,89.25,51,85.65,53.7"", ""M45.88,60.25c0.87,1,1.31,2.45,1.4,2.97c0.07,0.4,0,8.27-0.08,13.8c-0.02,1.64-0.04,2.48-0.06,3.22"", ""M48.28,61.54c7.47-0.86,27.74-3.55,29.36-3.64c2.31-0.13,3.73,0.48,3.73,2.72c0,6.76-0.37,12.13-0.81,17.47c-0.51,6.28-4.2,0.04-5.08-0.87"", ""M61.84,50.88c0.91,1.12,1.15,2.12,1.15,3.47c0,0.44-0.01,27.54-0.08,38.66c-0.01,2.19-0.03,3.72-0.04,4.25""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_05e30"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:05e30"" kvg:element=""帰"">
	<g id=""kvg:05e30-g1"" kvg:element=""刂"" kvg:variant=""true"" kvg:original=""刀"" kvg:position=""left"">
		<path id=""kvg:05e30-s1"" kvg:type=""㇑"" d=""M16.46,35.12c0.71,0.71,1.26,1.65,1.26,2.99c0,5.61-0.08,13.76-0.12,19.77c-0.01,1.77-0.02,3.35-0.02,4.63""/>
		<path id=""kvg:05e30-s2"" kvg:type=""㇒/㇚"" d=""M26.46,17.62c0.84,0.84,1.23,2.1,1.26,3.74c0.66,43.52-0.21,51.27-10.21,62.89""/>
	</g>
	<g id=""kvg:05e30-g2"" kvg:element=""帚"" kvg:position=""right"">
		<g id=""kvg:05e30-g3"" kvg:element=""⺕"" kvg:variant=""true"" kvg:original=""彑"" kvg:radical=""nelson"">
			<path id=""kvg:05e30-s3"" kvg:type=""㇕"" d=""M45.75,15.5c1.01,0.24,2.51,0.63,4.18,0.43C57.5,15,71.38,13.38,78,12.25c1.52-0.26,3.42,0.75,3.05,2.44c-1.03,4.73-2.29,13.19-3.06,18.34c-0.22,1.49-0.41,2.74-0.54,3.62""/>
			<path id=""kvg:05e30-s4"" kvg:type=""㇐c"" d=""M44.89,26.32c1.47,0.38,3.01,0.52,4.49,0.38c10.59-1,21.15-1.96,29.47-2.4""/>
			<path id=""kvg:05e30-s5"" kvg:type=""㇐c"" d=""M43.68,37.85c1.65,0.5,3.41,0.49,5.08,0.27c8.74-1.13,19.84-2.66,27.98-3.19""/>
		</g>
		<g id=""kvg:05e30-g4"" kvg:element=""冖"">
			<path id=""kvg:05e30-s6"" kvg:type=""㇔"" d=""M40.88,48.75c-0.09,3.63-2.32,9.83-3.12,12""/>
			<path id=""kvg:05e30-s7"" kvg:type=""㇖b"" d=""M41.49,51.15c11.51-1.9,35.42-5.38,45.63-5.93C96,44.75,89.25,51,85.65,53.7""/>
		</g>
		<g id=""kvg:05e30-g5"" kvg:element=""巾"" kvg:radical=""tradit"">
			<path id=""kvg:05e30-s8"" kvg:type=""㇑"" d=""M45.88,60.25c0.87,1,1.31,2.45,1.4,2.97c0.07,0.4,0,8.27-0.08,13.8c-0.02,1.64-0.04,2.48-0.06,3.22""/>
			<path id=""kvg:05e30-s9"" kvg:type=""㇆a"" d=""M48.28,61.54c7.47-0.86,27.74-3.55,29.36-3.64c2.31-0.13,3.73,0.48,3.73,2.72c0,6.76-0.37,12.13-0.81,17.47c-0.51,6.28-4.2,0.04-5.08-0.87""/>
			<path id=""kvg:05e30-s10"" kvg:type=""㇑"" d=""M61.84,50.88c0.91,1.12,1.15,2.12,1.15,3.47c0,0.44-0.01,27.54-0.08,38.66c-0.01,2.19-0.03,3.72-0.04,4.25""/>
		</g>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_05e30"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 6.75 34.63)"">1</text>
	<text transform=""matrix(1 0 0 1 17.25 18.13)"">2</text>
	<text transform=""matrix(1 0 0 1 38.25 16.63)"">3</text>
	<text transform=""matrix(1 0 0 1 36.75 28.63)"">4</text>
	<text transform=""matrix(1 0 0 1 36.75 40.63)"">5</text>
	<text transform=""matrix(1 0 0 1 33.75 52.63)"">6</text>
	<text transform=""matrix(1 0 0 1 42.75 49.63)"">7</text>
	<text transform=""matrix(1 0 0 1 39.75 69.13)"">8</text>
	<text transform=""matrix(1 0 0 1 50.25 58.63)"">9</text>
	<text transform=""matrix(1 0 0 1 66.75 55.63)"">10</text>
</g>
</svg>
",
                OrderIndex = 3,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("354fb615-4b3c-5370-bd15-bcaa1c1b81bc"),
                LessonId = lesson6Id,
                Level = "N3",
                Character = "会",
                HanViet = "HỘI",
                Meaning = "Gặp gỡ, hội họp",
                StrokeCount = 6,
                KunReading = "あ.う",
                OnReading = "カイ、エ",
                Mnemonic = "Hai (二) người (𠆢) lén lút gặp gỡ ở nơi riêng tư (厶) để bàn chuyện đại hội (会).",
                ComponentMapJson = @"[{""character"": ""二"", ""component"": ""二"", ""name"": ""二"", ""meaning"": ""二""}, {""character"": ""厶"", ""component"": ""厶"", ""name"": ""厶"", ""meaning"": ""厶""}]",
                StrokeDataJson = @"[""M52.25,14c0.25,2.28-0.52,3.59-1.8,5.62c-5.76,9.14-17.9,27-39.2,39.88"", ""M54.5,19.25c6.73,7.3,24.09,24.81,32.95,31.91c2.73,2.18,5.61,3.8,9.05,4.59"", ""M37.36,50.16c1.64,0.34,4.04,0.36,4.98,0.25c6.79-0.79,14.29-1.91,19.66-2.4c1.56-0.14,3.25-0.39,4.66,0"", ""M23,65.98c2.12,0.52,4.25,0.64,7.01,0.3c13.77-1.71,30.99-3.66,46.35-3.74c3.04-0.02,4.87,0.14,6.4,0.29"", ""M47.16,66.38c0.62,1.65-0.03,2.93-0.92,4.28c-5.17,7.8-8.02,11.38-14.99,18.84c-2.11,2.25-1.5,4.18,2,3.75c7.35-0.91,28.19-5.83,40.16-7.95"", ""M66.62,77.39c4.52,3.23,11,12.73,13.06,18.82""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_04f1a"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:04f1a"" kvg:element=""会"">
	<g id=""kvg:04f1a-g1"" kvg:element=""人"" kvg:position=""top"" kvg:radical=""general"">
		<path id=""kvg:04f1a-s1"" kvg:type=""㇒"" d=""M52.25,14c0.25,2.28-0.52,3.59-1.8,5.62c-5.76,9.14-17.9,27-39.2,39.88""/>
		<path id=""kvg:04f1a-s2"" kvg:type=""㇏"" d=""M54.5,19.25c6.73,7.3,24.09,24.81,32.95,31.91c2.73,2.18,5.61,3.8,9.05,4.59""/>
	</g>
	<g id=""kvg:04f1a-g2"" kvg:element=""云"" kvg:position=""bottom"">
		<g id=""kvg:04f1a-g3"" kvg:element=""二"">
			<path id=""kvg:04f1a-s3"" kvg:type=""㇐"" d=""M37.36,50.16c1.64,0.34,4.04,0.36,4.98,0.25c6.79-0.79,14.29-1.91,19.66-2.4c1.56-0.14,3.25-0.39,4.66,0""/>
			<path id=""kvg:04f1a-s4"" kvg:type=""㇐"" d=""M23,65.98c2.12,0.52,4.25,0.64,7.01,0.3c13.77-1.71,30.99-3.66,46.35-3.74c3.04-0.02,4.87,0.14,6.4,0.29""/>
		</g>
		<g id=""kvg:04f1a-g4"" kvg:element=""厶"">
			<path id=""kvg:04f1a-s5"" kvg:type=""㇜"" d=""M47.16,66.38c0.62,1.65-0.03,2.93-0.92,4.28c-5.17,7.8-8.02,11.38-14.99,18.84c-2.11,2.25-1.5,4.18,2,3.75c7.35-0.91,28.19-5.83,40.16-7.95""/>
			<path id=""kvg:04f1a-s6"" kvg:type=""㇔"" d=""M66.62,77.39c4.52,3.23,11,12.73,13.06,18.82""/>
		</g>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_04f1a"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 44.25 10.63)"">1</text>
	<text transform=""matrix(1 0 0 1 60.75 21.13)"">2</text>
	<text transform=""matrix(1 0 0 1 39.75 46.63)"">3</text>
	<text transform=""matrix(1 0 0 1 23.25 63.13)"">4</text>
	<text transform=""matrix(1 0 0 1 33.75 76.63)"">5</text>
	<text transform=""matrix(1 0 0 1 69.75 76.63)"">6</text>
</g>
</svg>
",
                OrderIndex = 4,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("dcacdd9c-da20-511a-a01c-3ab61322d652"),
                LessonId = lesson6Id,
                Level = "N3",
                Character = "社",
                HanViet = "XÃ",
                Meaning = "Xã hội, công ty, đền thờ",
                StrokeCount = 7,
                KunReading = "やしろ",
                OnReading = "シャ",
                Mnemonic = "Lập bàn thờ thần (礻) trên bãi đất (土) rộng để cúng tế cho cả xã hội (社).",
                ComponentMapJson = @"[{""character"": ""礻"", ""component"": ""礻"", ""name"": ""礻"", ""meaning"": ""礻""}, {""character"": ""土"", ""component"": ""土"", ""name"": ""土"", ""meaning"": ""土""}]",
                StrokeDataJson = @"[""M29.5,15.25c3.41,2.06,6.75,4.75,10,8.75"", ""M15.5,39c2,0.75,3.05,0.8,5.55,0.17c6.2-1.55,15.6-4.05,17.7-4.67c2.5-0.75,4.46,1.23,3,3.5c-4.75,7.38-13.88,18.62-25,28.75"", ""M30.82,55.37C32,56.5,32.21,58,32.21,59.94c0,8.57-0.15,21.05-0.3,29.06c-0.06,3.28-0.12,5.81-0.12,7"", ""M36.25,54.5c3.67,2.41,7.6,6.03,10.75,10.25"", ""M52.13,52.03c1.49,0.39,4.23,0.31,5.71,0.14c7.92-0.91,19.17-2.04,28.02-2.68c2.48-0.18,3.97-0.07,5.22,0.13"", ""M69.31,17.87c1.23,1.23,1.81,3.13,1.81,5.13c0,14.25,0.13,62.57,0.13,63.07"", ""M42.25,88.97c1.92,0.55,5.44,0.76,7.36,0.55c12.39-1.39,28.51-3.64,41.04-4.03c3.2-0.1,6.09,0.14,7.72,0.79""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_0793e"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:0793e"" kvg:element=""社"">
	<g id=""kvg:0793e-g1"" kvg:element=""礻"" kvg:variant=""true"" kvg:original=""示"" kvg:position=""left"" kvg:radical=""general"">
		<path id=""kvg:0793e-s1"" kvg:type=""㇔"" d=""M29.5,15.25c3.41,2.06,6.75,4.75,10,8.75""/>
		<path id=""kvg:0793e-s2"" kvg:type=""㇇"" d=""M15.5,39c2,0.75,3.05,0.8,5.55,0.17c6.2-1.55,15.6-4.05,17.7-4.67c2.5-0.75,4.46,1.23,3,3.5c-4.75,7.38-13.88,18.62-25,28.75""/>
		<path id=""kvg:0793e-s3"" kvg:type=""㇑"" d=""M30.82,55.37C32,56.5,32.21,58,32.21,59.94c0,8.57-0.15,21.05-0.3,29.06c-0.06,3.28-0.12,5.81-0.12,7""/>
		<path id=""kvg:0793e-s4"" kvg:type=""㇔"" d=""M36.25,54.5c3.67,2.41,7.6,6.03,10.75,10.25""/>
	</g>
	<g id=""kvg:0793e-g2"" kvg:element=""土"" kvg:position=""right"" kvg:phon=""土"">
		<path id=""kvg:0793e-s5"" kvg:type=""㇐"" d=""M52.13,52.03c1.49,0.39,4.23,0.31,5.71,0.14c7.92-0.91,19.17-2.04,28.02-2.68c2.48-0.18,3.97-0.07,5.22,0.13""/>
		<path id=""kvg:0793e-s6"" kvg:type=""㇑a"" d=""M69.31,17.87c1.23,1.23,1.81,3.13,1.81,5.13c0,14.25,0.13,62.57,0.13,63.07""/>
		<path id=""kvg:0793e-s7"" kvg:type=""㇐"" d=""M42.25,88.97c1.92,0.55,5.44,0.76,7.36,0.55c12.39-1.39,28.51-3.64,41.04-4.03c3.2-0.1,6.09,0.14,7.72,0.79""/>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_0793e"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 21.75 13.63)"">1</text>
	<text transform=""matrix(1 0 0 1 8.50 40.50)"">2</text>
	<text transform=""matrix(1 0 0 1 24.75 68.50)"">3</text>
	<text transform=""matrix(1 0 0 1 38.50 53.50)"">4</text>
	<text transform=""matrix(1 0 0 1 50.50 48.13)"">5</text>
	<text transform=""matrix(1 0 0 1 59.50 17.50)"">6</text>
	<text transform=""matrix(1 0 0 1 40.50 85.63)"">7</text>
</g>
</svg>
",
                OrderIndex = 5,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("825fb13d-2b5f-5a47-aefc-0fe1c0c3c290"),
                LessonId = lesson6Id,
                Level = "N3",
                Character = "聞",
                HanViet = "VĂN",
                Meaning = "Nghe, hỏi",
                StrokeCount = 14,
                KunReading = "き.く、き.こえる",
                OnReading = "ブン、モン",
                Mnemonic = "Ghé sát tai (耳) vào cổng (門) để nghe ngóng và hỏi thăm tin tức.",
                ComponentMapJson = @"[{""character"": ""耳"", ""component"": ""耳"", ""name"": ""耳"", ""meaning"": ""耳""}, {""character"": ""門"", ""component"": ""門"", ""name"": ""門"", ""meaning"": ""門""}, {""character"": ""口"", ""component"": ""口"", ""name"": ""口"", ""meaning"": ""口""}, {""character"": ""門"", ""component"": ""門"", ""name"": ""門"", ""meaning"": ""門""}]",
                StrokeDataJson = @"[""M16.14,17.97c0.94,0.94,1.51,2.16,1.51,3.25c0,0.77-0.03,48.45-0.18,66.29c-0.03,3.16-0.05,5.37-0.07,6.21"", ""M19.01,19.6c6.86-1.22,18.49-3.1,21.08-3.39c1.9-0.21,3.03,0.79,3,2.46c-0.04,1.84-0.59,10.46-1.44,20.02c-0.09,1.04-0.15,2-0.15,2.69"", ""M18.81,30.43c6.94-1.05,15.82-2.55,22.41-2.9"", ""M17.86,42.57C26.25,41.25,33,40,40.42,39.4"", ""M66.21,14.22c0.66,0.66,1.17,1.78,1.17,2.93c0,0.56,0.12,13.19,0.17,19.1c0.02,1.71,0.03,2.83,0.03,2.91"", ""M68.51,16.1c6.89-1.1,19.28-3.26,21.17-3.36c1.96-0.1,3.57,1.38,3.57,2.98c0,18.78-0.26,60.28-0.26,73.89c0,11.13-6.37,2.13-8.21,0.25"", ""M68.59,25.94c5.16-0.69,18.16-2.44,23-2.71"", ""M69.13,36.75c6.37-0.75,15.12-2,22.15-2.53"", ""M33.5,51.24c1.71,0.31,4.02,0.25,5.71,0.06c9.6-1.05,20.21-3.05,30.66-4.05c2.83-0.27,4.57-0.1,6,0.05"", ""M42.71,53c0.89,0.89,1.3,2.26,1.3,3.51S44,79.31,44,83.84"", ""M45.13,61.43c4.49-0.43,13.74-2.05,19.78-2.36"", ""M44.81,71.67c5.72-0.67,12.31-1.92,20.12-2.95"", ""M32.51,85.14c0.99,0.86,2.2,1.23,3.25,0.92c4.87-1.43,28.2-7.83,34.76-9.39"", ""M64.84,49.66c0.62,0.63,1.05,1.71,1.05,2.99c0,0.66,0.12,27.55,0.15,39.1c0.01,1.88,0.01,3.36,0.01,4.25""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_0805e"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:0805e"" kvg:element=""聞"">
	<g id=""kvg:0805e-g1"" kvg:element=""門"" kvg:position=""kamae"" kvg:radical=""nelson"" kvg:phon=""門"">
		<g id=""kvg:0805e-g2"" kvg:position=""left"">
			<path id=""kvg:0805e-s1"" kvg:type=""㇑"" d=""M16.14,17.97c0.94,0.94,1.51,2.16,1.51,3.25c0,0.77-0.03,48.45-0.18,66.29c-0.03,3.16-0.05,5.37-0.07,6.21""/>
			<path id=""kvg:0805e-s2"" kvg:type=""㇕a"" d=""M19.01,19.6c6.86-1.22,18.49-3.1,21.08-3.39c1.9-0.21,3.03,0.79,3,2.46c-0.04,1.84-0.59,10.46-1.44,20.02c-0.09,1.04-0.15,2-0.15,2.69""/>
			<path id=""kvg:0805e-s3"" kvg:type=""㇐a"" d=""M18.81,30.43c6.94-1.05,15.82-2.55,22.41-2.9""/>
			<path id=""kvg:0805e-s4"" kvg:type=""㇐a"" d=""M17.86,42.57C26.25,41.25,33,40,40.42,39.4""/>
		</g>
		<g id=""kvg:0805e-g3"" kvg:position=""right"">
			<path id=""kvg:0805e-s5"" kvg:type=""㇑"" d=""M66.21,14.22c0.66,0.66,1.17,1.78,1.17,2.93c0,0.56,0.12,13.19,0.17,19.1c0.02,1.71,0.03,2.83,0.03,2.91""/>
			<path id=""kvg:0805e-s6"" kvg:type=""㇆a"" d=""M68.51,16.1c6.89-1.1,19.28-3.26,21.17-3.36c1.96-0.1,3.57,1.38,3.57,2.98c0,18.78-0.26,60.28-0.26,73.89c0,11.13-6.37,2.13-8.21,0.25""/>
			<path id=""kvg:0805e-s7"" kvg:type=""㇐a"" d=""M68.59,25.94c5.16-0.69,18.16-2.44,23-2.71""/>
			<path id=""kvg:0805e-s8"" kvg:type=""㇐a"" d=""M69.13,36.75c6.37-0.75,15.12-2,22.15-2.53""/>
		</g>
	</g>
	<g id=""kvg:0805e-g4"" kvg:element=""耳"" kvg:radical=""tradit"">
		<path id=""kvg:0805e-s9"" kvg:type=""㇐"" d=""M33.5,51.24c1.71,0.31,4.02,0.25,5.71,0.06c9.6-1.05,20.21-3.05,30.66-4.05c2.83-0.27,4.57-0.1,6,0.05""/>
		<path id=""kvg:0805e-s10"" kvg:type=""㇑a"" d=""M42.71,53c0.89,0.89,1.3,2.26,1.3,3.51S44,79.31,44,83.84""/>
		<path id=""kvg:0805e-s11"" kvg:type=""㇐a"" d=""M45.13,61.43c4.49-0.43,13.74-2.05,19.78-2.36""/>
		<path id=""kvg:0805e-s12"" kvg:type=""㇐a"" d=""M44.81,71.67c5.72-0.67,12.31-1.92,20.12-2.95""/>
		<path id=""kvg:0805e-s13"" kvg:type=""㇀"" d=""M32.51,85.14c0.99,0.86,2.2,1.23,3.25,0.92c4.87-1.43,28.2-7.83,34.76-9.39""/>
		<path id=""kvg:0805e-s14"" kvg:type=""㇑"" d=""M64.84,49.66c0.62,0.63,1.05,1.71,1.05,2.99c0,0.66,0.12,27.55,0.15,39.1c0.01,1.88,0.01,3.36,0.01,4.25""/>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_0805e"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 10.50 26.50)"">1</text>
	<text transform=""matrix(1 0 0 1 19.50 16.50)"">2</text>
	<text transform=""matrix(1 0 0 1 21.50 27.13)"">3</text>
	<text transform=""matrix(1 0 0 1 21.50 39.13)"">4</text>
	<text transform=""matrix(1 0 0 1 59.50 21.50)"">5</text>
	<text transform=""matrix(1 0 0 1 69.50 12.50)"">6</text>
	<text transform=""matrix(1 0 0 1 71.50 23.50)"">7</text>
	<text transform=""matrix(1 0 0 1 71.50 34.63)"">8</text>
	<text transform=""matrix(1 0 0 1 26.25 53.50)"">9</text>
	<text transform=""matrix(1 0 0 1 33.50 61.50)"">10</text>
	<text transform=""matrix(1 0 0 1 47.50 58.50)"">11</text>
	<text transform=""matrix(1 0 0 1 47.50 68.50)"">12</text>
	<text transform=""matrix(1 0 0 1 23.50 92.50)"">13</text>
	<text transform=""matrix(1 0 0 1 56.50 55.50)"">14</text>
</g>
</svg>
",
                OrderIndex = 6,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("2d9ed815-cb0f-5465-9d86-9c9c69412804"),
                LessonId = lesson6Id,
                Level = "N3",
                Character = "読",
                HanViet = "ĐỘC",
                Meaning = "Đọc",
                StrokeCount = 14,
                KunReading = "よ.む",
                OnReading = "ドク、トク",
                Mnemonic = "Dùng ngôn từ (言) để rao bán (売) cuốn sách mà mình đang đọc (読).",
                ComponentMapJson = @"[{""character"": ""言"", ""component"": ""言"", ""name"": ""言"", ""meaning"": ""言""}, {""character"": ""売"", ""component"": ""売"", ""name"": ""売"", ""meaning"": ""売""}]",
                StrokeDataJson = @"[""M22.38,14.75c2.25,1.63,5.81,6.71,6.37,9.25"", ""M10.37,33.08c1.61,0.48,3.62,0.35,5.27,0.14c5.96-0.76,13.52-1.42,20.1-2.38c1.5-0.22,3.09-0.43,4.6-0.16"", ""M16.23,46.31c1.17,0.37,2.73,0.18,3.93-0.01c3.99-0.62,8.33-1.2,11.58-1.97c1.35-0.32,3.26-0.58,4.65-0.58"", ""M16.73,58.58c1.02,0.35,2.46,0.15,3.53,0.04c3.8-0.4,9.57-1.17,12.55-1.77c1.45-0.29,2.94-0.48,4.22-0.14"", ""M15.64,70.4c0.71,0.61,1.08,1.37,1.12,2.29c0.79,3.76,1.71,9.85,2.52,15.05c0.16,1.05,0.32,2.06,0.48,3"", ""M17.75,72.05c6.09-0.91,11.59-1.7,17.42-2.67c1.7-0.28,2.73,1.3,2.49,2.58c-0.85,4.46-1.61,6.91-2.88,12.78"", ""M20.47,88.3c4.06-0.46,7.76-1.19,12.79-1.92c0.92-0.13,1.88-0.26,2.9-0.39"", ""M46.37,26.71c2.31,0.59,4.67,0.42,7,0.15c9.97-1.14,21.82-2.23,29.77-3.06c2.36-0.25,4.38-0.21,6.71,0.14"", ""M65.76,13.25c1.06,1.06,1.59,2.08,1.59,3.25c0,8.5-0.07,17.03-0.19,20.46"", ""M52.53,38.58c2.22,0.42,4.15,0.3,5.98,0.08C64,38,71.21,37.1,78,36.47c1.61-0.15,3.63-0.29,5.23,0.04"", ""M46.14,48.1c-0.11,3.93-1.7,12-2.6,14.35"", ""M47,49.96c11.42-1.27,28-3.71,41.35-4.28c9.15-0.39,0.43,7.14-0.74,8.35"", ""M59.49,59.25c0.5,1.52,0.71,3.09,0.29,4.83c-2.76,11.61-6.54,22.94-15.26,30.61"", ""M71.64,56.26c1.11,1.24,1.65,2.67,1.69,4.37c0.11,4.62-0.13,16.99-0.13,24.62c0,9.5,0.8,10.52,11.3,10.52c11,0,11.38-1.02,11.38-7.31""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_08aad"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:08aad"" kvg:element=""読"">
	<g id=""kvg:08aad-g1"" kvg:element=""言"" kvg:position=""left"" kvg:radical=""general"">
		<path id=""kvg:08aad-s1"" kvg:type=""㇔"" d=""M22.38,14.75c2.25,1.63,5.81,6.71,6.37,9.25""/>
		<path id=""kvg:08aad-s2"" kvg:type=""㇐"" d=""M10.37,33.08c1.61,0.48,3.62,0.35,5.27,0.14c5.96-0.76,13.52-1.42,20.1-2.38c1.5-0.22,3.09-0.43,4.6-0.16""/>
		<path id=""kvg:08aad-s3"" kvg:type=""㇐"" d=""M16.23,46.31c1.17,0.37,2.73,0.18,3.93-0.01c3.99-0.62,8.33-1.2,11.58-1.97c1.35-0.32,3.26-0.58,4.65-0.58""/>
		<path id=""kvg:08aad-s4"" kvg:type=""㇐"" d=""M16.73,58.58c1.02,0.35,2.46,0.15,3.53,0.04c3.8-0.4,9.57-1.17,12.55-1.77c1.45-0.29,2.94-0.48,4.22-0.14""/>
		<g id=""kvg:08aad-g2"" kvg:element=""口"">
			<path id=""kvg:08aad-s5"" kvg:type=""㇑"" d=""M15.64,70.4c0.71,0.61,1.08,1.37,1.12,2.29c0.79,3.76,1.71,9.85,2.52,15.05c0.16,1.05,0.32,2.06,0.48,3""/>
			<path id=""kvg:08aad-s6"" kvg:type=""㇕b"" d=""M17.75,72.05c6.09-0.91,11.59-1.7,17.42-2.67c1.7-0.28,2.73,1.3,2.49,2.58c-0.85,4.46-1.61,6.91-2.88,12.78""/>
			<path id=""kvg:08aad-s7"" kvg:type=""㇐b"" d=""M20.47,88.3c4.06-0.46,7.76-1.19,12.79-1.92c0.92-0.13,1.88-0.26,2.9-0.39""/>
		</g>
	</g>
	<g id=""kvg:08aad-g3"" kvg:element=""売"" kvg:position=""right"" kvg:phon=""売"">
		<g id=""kvg:08aad-g4"" kvg:element=""士"" kvg:position=""top"">
			<path id=""kvg:08aad-s8"" kvg:type=""㇐"" d=""M46.37,26.71c2.31,0.59,4.67,0.42,7,0.15c9.97-1.14,21.82-2.23,29.77-3.06c2.36-0.25,4.38-0.21,6.71,0.14""/>
			<path id=""kvg:08aad-s9"" kvg:type=""㇑a"" d=""M65.76,13.25c1.06,1.06,1.59,2.08,1.59,3.25c0,8.5-0.07,17.03-0.19,20.46""/>
			<path id=""kvg:08aad-s10"" kvg:type=""㇐"" d=""M52.53,38.58c2.22,0.42,4.15,0.3,5.98,0.08C64,38,71.21,37.1,78,36.47c1.61-0.15,3.63-0.29,5.23,0.04""/>
		</g>
		<g id=""kvg:08aad-g5"" kvg:position=""bottom"">
			<g id=""kvg:08aad-g6"" kvg:element=""冖"">
				<path id=""kvg:08aad-s11"" kvg:type=""㇔"" d=""M46.14,48.1c-0.11,3.93-1.7,12-2.6,14.35""/>
				<path id=""kvg:08aad-s12"" kvg:type=""㇖b"" d=""M47,49.96c11.42-1.27,28-3.71,41.35-4.28c9.15-0.39,0.43,7.14-0.74,8.35""/>
			</g>
			<g id=""kvg:08aad-g7"" kvg:element=""儿"" kvg:original=""八"">
				<g id=""kvg:08aad-g8"" kvg:element=""丿"">
					<path id=""kvg:08aad-s13"" kvg:type=""㇒"" d=""M59.49,59.25c0.5,1.52,0.71,3.09,0.29,4.83c-2.76,11.61-6.54,22.94-15.26,30.61""/>
				</g>
				<path id=""kvg:08aad-s14"" kvg:type=""㇟"" d=""M71.64,56.26c1.11,1.24,1.65,2.67,1.69,4.37c0.11,4.62-0.13,16.99-0.13,24.62c0,9.5,0.8,10.52,11.3,10.52c11,0,11.38-1.02,11.38-7.31""/>
			</g>
		</g>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_08aad"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 15.50 15.50)"">1</text>
	<text transform=""matrix(1 0 0 1 3.75 34.63)"">2</text>
	<text transform=""matrix(1 0 0 1 9.50 48.13)"">3</text>
	<text transform=""matrix(1 0 0 1 9.50 61.63)"">4</text>
	<text transform=""matrix(1 0 0 1 8.50 76.63)"">5</text>
	<text transform=""matrix(1 0 0 1 18.50 69.50)"">6</text>
	<text transform=""matrix(1 0 0 1 22.50 85.50)"">7</text>
	<text transform=""matrix(1 0 0 1 45.50 24.50)"">8</text>
	<text transform=""matrix(1 0 0 1 58.50 13.50)"">9</text>
	<text transform=""matrix(1 0 0 1 42.50 36.50)"">10</text>
	<text transform=""matrix(1 0 0 1 37.50 51.50)"">11</text>
	<text transform=""matrix(1 0 0 1 46.50 46.63)"">12</text>
	<text transform=""matrix(1 0 0 1 49.50 65.50)"">13</text>
	<text transform=""matrix(1 0 0 1 76.50 63.50)"">14</text>
</g>
</svg>
",
                OrderIndex = 7,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("af7bb69e-c0ff-5d82-b2fc-ace3112451cb"),
                LessonId = lesson6Id,
                Level = "N3",
                Character = "書",
                HanViet = "THƯ",
                Meaning = "Viết, sách",
                StrokeCount = 10,
                KunReading = "か.く",
                OnReading = "ショ",
                Mnemonic = "Cầm cây bút (聿) mải mê viết sách (書) suốt cả một ngày (日) dài.",
                ComponentMapJson = @"[{""character"": ""聿"", ""component"": ""聿"", ""name"": ""聿"", ""meaning"": ""聿""}, {""character"": ""日"", ""component"": ""日"", ""name"": ""日"", ""meaning"": ""日""}]",
                StrokeDataJson = @"[""M30.74,20.63c2.01,0.49,3.84,0.58,5.91,0.39c9.45-0.89,28.54-2.97,37.54-3.64c2.92-0.22,4.18,1.24,3.66,3.55c-0.4,1.76-2.56,9.62-3.88,16.56"", ""M11.89,32.34c3.17,0.66,5.87,0.47,9.58,0.19c20.16-1.52,48.41-3.9,67.91-4.64c4.08-0.16,7.07,0.26,8.91,0.59"", ""M29.86,41.38c1.64,0.49,3.39,0.52,4.84,0.44c9.67-0.57,27.55-2.07,37.43-2.85c1.93-0.15,3.14-0.08,4.59,0.07"", ""M30.04,52.06c1.47,0.26,3.65,0.54,5.12,0.43c12.34-0.98,25.59-2.23,36.78-3.31c2.43-0.23,4.41-0.19,5.63-0.07"", ""M17,63.44c2,0.58,5.67,0.57,7.66,0.41c20.78-1.7,42.92-3.93,61.09-4.48c3.33-0.1,5.33,0.15,6.99,0.42"", ""M52.69,9.52c1.33,1.33,1.95,2.98,1.95,4.71c0,5.67,0.22,33.72,0.31,45.77"", ""M31.25,71.75c0.43,0.46,1.24,1.44,1.43,2.7c1.11,7.42,2.22,14.33,3.18,20.74c0.18,1.2,0.34,2.38,0.49,3.57"", ""M33.75,73.75c14.98-2.05,36.1-3.71,44.41-4.26c3.33-0.22,5.09,1.76,4.6,4.33c-0.99,5.22-2.01,11.55-3.81,19.41c-0.3,1.31-0.74,2.55-1.15,3.72"", ""M35.5,83.75C46.79,83.03,65.75,81.5,80.25,81"", ""M37.25,95.25c10.7-0.68,26.5-1.38,40.5-2""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_066f8"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:066f8"" kvg:element=""書"">
	<g id=""kvg:066f8-g1"" kvg:element=""聿"" kvg:position=""top"" kvg:radical=""nelson"">
		<g id=""kvg:066f8-g2"" kvg:element=""⺕"" kvg:variant=""true"" kvg:original=""彑"">
			<path id=""kvg:066f8-s1"" kvg:type=""㇕c"" d=""M30.74,20.63c2.01,0.49,3.84,0.58,5.91,0.39c9.45-0.89,28.54-2.97,37.54-3.64c2.92-0.22,4.18,1.24,3.66,3.55c-0.4,1.76-2.56,9.62-3.88,16.56""/>
			<path id=""kvg:066f8-s2"" kvg:type=""㇐"" d=""M11.89,32.34c3.17,0.66,5.87,0.47,9.58,0.19c20.16-1.52,48.41-3.9,67.91-4.64c4.08-0.16,7.07,0.26,8.91,0.59""/>
			<path id=""kvg:066f8-s3"" kvg:type=""㇐"" d=""M29.86,41.38c1.64,0.49,3.39,0.52,4.84,0.44c9.67-0.57,27.55-2.07,37.43-2.85c1.93-0.15,3.14-0.08,4.59,0.07""/>
		</g>
		<path id=""kvg:066f8-s4"" kvg:type=""㇐"" d=""M30.04,52.06c1.47,0.26,3.65,0.54,5.12,0.43c12.34-0.98,25.59-2.23,36.78-3.31c2.43-0.23,4.41-0.19,5.63-0.07""/>
		<path id=""kvg:066f8-s5"" kvg:type=""㇐"" d=""M17,63.44c2,0.58,5.67,0.57,7.66,0.41c20.78-1.7,42.92-3.93,61.09-4.48c3.33-0.1,5.33,0.15,6.99,0.42""/>
		<g id=""kvg:066f8-g3"" kvg:element=""丨"">
			<path id=""kvg:066f8-s6"" kvg:type=""㇑a"" d=""M52.69,9.52c1.33,1.33,1.95,2.98,1.95,4.71c0,5.67,0.22,33.72,0.31,45.77""/>
		</g>
	</g>
	<g id=""kvg:066f8-g4"" kvg:element=""日"" kvg:original=""曰"" kvg:position=""bottom"" kvg:radical=""tradit"" kvg:phon=""者T"">
		<path id=""kvg:066f8-s7"" kvg:type=""㇑"" d=""M31.25,71.75c0.43,0.46,1.24,1.44,1.43,2.7c1.11,7.42,2.22,14.33,3.18,20.74c0.18,1.2,0.34,2.38,0.49,3.57""/>
		<path id=""kvg:066f8-s8"" kvg:type=""㇕a"" d=""M33.75,73.75c14.98-2.05,36.1-3.71,44.41-4.26c3.33-0.22,5.09,1.76,4.6,4.33c-0.99,5.22-2.01,11.55-3.81,19.41c-0.3,1.31-0.74,2.55-1.15,3.72""/>
		<path id=""kvg:066f8-s9"" kvg:type=""㇐a"" d=""M35.5,83.75C46.79,83.03,65.75,81.5,80.25,81""/>
		<path id=""kvg:066f8-s10"" kvg:type=""㇐a"" d=""M37.25,95.25c10.7-0.68,26.5-1.38,40.5-2""/>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_066f8"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 23.50 21.50)"">1</text>
	<text transform=""matrix(1 0 0 1 4.50 33.50)"">2</text>
	<text transform=""matrix(1 0 0 1 22.50 41.50)"">3</text>
	<text transform=""matrix(1 0 0 1 21.75 52.50)"">4</text>
	<text transform=""matrix(1 0 0 1 9.50 64.50)"">5</text>
	<text transform=""matrix(1 0 0 1 42.50 10.50)"">6</text>
	<text transform=""matrix(1 0 0 1 23.50 77.50)"">7</text>
	<text transform=""matrix(1 0 0 1 35.50 70.50)"">8</text>
	<text transform=""matrix(1 0 0 1 38.50 81.50)"">9</text>
	<text transform=""matrix(1 0 0 1 38.50 92.50)"">10</text>
</g>
</svg>
",
                OrderIndex = 8,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("823b9d3b-c02f-5bab-bba7-12f8c7751748"),
                LessonId = lesson6Id,
                Level = "N3",
                Character = "話",
                HanViet = "THOẠI",
                Meaning = "Trò chuyện, câu chuyện",
                StrokeCount = 13,
                KunReading = "はな.す、はなし",
                OnReading = "ワ",
                Mnemonic = "Kết hợp ngôn từ (言) khéo léo và uốn cái lưỡi (舌) để trò chuyện (話) với mọi người.",
                ComponentMapJson = @"[{""character"": ""言"", ""component"": ""言"", ""name"": ""言"", ""meaning"": ""言""}, {""character"": ""舌"", ""component"": ""舌"", ""name"": ""舌"", ""meaning"": ""舌""}]",
                StrokeDataJson = @"[""M24.99,14c2.36,1.5,6.1,6.17,6.7,8.5"", ""M11.37,32.83c1.41,0.42,3.07,0.29,4.51,0.17c8.29-0.7,16.95-1.9,23.59-2.81c1.28-0.17,3.22,0.11,3.87,0.23"", ""M17.78,47.06c1.05,0.32,2.15,0.35,3.23,0.29c4.11-0.23,10.69-1.59,14.43-1.97c1.31-0.13,2.68-0.13,4.04-0.13"", ""M18.58,59.33c1.2,0.36,2.8,0.13,4.04,0.08c3.71-0.16,8.38-0.79,12.92-1.36c1.43-0.18,2.85-0.34,4.3-0.08"", ""M17.4,71.9c0.81,0.68,1.33,1.82,1.49,2.87c0.73,4.61,1.4,8.31,2.2,13.18c0.22,1.33,0.43,2.62,0.63,3.8"", ""M19.64,73.54c6.67-1.14,13.31-2.75,19.46-3.66c1.85-0.27,2.96,1.26,2.7,2.51c-0.89,4.19-2.46,8.16-4.05,14.07"", ""M22.49,89.55c4.76-0.55,8.86-1.17,14.02-1.72c0.93-0.1,1.91,0.04,2.97-0.09"", ""M81.75,13.75c-0.12,1.25-0.79,2.39-1.66,3.06c-4.84,3.69-15.34,9.94-29.34,14.94"", ""M45.39,45.8c1.55,0.34,4.36,0.62,6.73,0.34C62.8,44.9,76.37,43.07,91,42.23c2.28-0.13,4.49-0.01,6.75,0.29"", ""M68.87,27.19c1.4,1.4,1.85,3.06,1.85,4.88c0,1.44-0.21,28.18-0.21,35.16"", ""M52.07,67.83c0.93,1.17,1.22,1.76,1.35,2.34c1.38,6.2,2.13,12.33,3.08,20.33c0.13,1.14,0.27,2.31,0.42,3.53"", ""M54.62,69.29c12.9-1.37,25.41-2.73,32.68-3.31c2.94-0.24,3.79,2.4,3.37,3.8c-1.47,4.87-3.03,11.64-5.04,18.35"", ""M56.64,91.74c7.5-0.57,16.99-1.13,27.14-1.93c1.37-0.11,2.75-0.22,4.13-0.34""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_08a71"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:08a71"" kvg:element=""話"">
	<g id=""kvg:08a71-g1"" kvg:element=""言"" kvg:position=""left"" kvg:radical=""general"">
		<path id=""kvg:08a71-s1"" kvg:type=""㇔"" d=""M24.99,14c2.36,1.5,6.1,6.17,6.7,8.5""/>
		<path id=""kvg:08a71-s2"" kvg:type=""㇐"" d=""M11.37,32.83c1.41,0.42,3.07,0.29,4.51,0.17c8.29-0.7,16.95-1.9,23.59-2.81c1.28-0.17,3.22,0.11,3.87,0.23""/>
		<path id=""kvg:08a71-s3"" kvg:type=""㇐"" d=""M17.78,47.06c1.05,0.32,2.15,0.35,3.23,0.29c4.11-0.23,10.69-1.59,14.43-1.97c1.31-0.13,2.68-0.13,4.04-0.13""/>
		<path id=""kvg:08a71-s4"" kvg:type=""㇐"" d=""M18.58,59.33c1.2,0.36,2.8,0.13,4.04,0.08c3.71-0.16,8.38-0.79,12.92-1.36c1.43-0.18,2.85-0.34,4.3-0.08""/>
		<g id=""kvg:08a71-g2"" kvg:element=""口"">
			<path id=""kvg:08a71-s5"" kvg:type=""㇑"" d=""M17.4,71.9c0.81,0.68,1.33,1.82,1.49,2.87c0.73,4.61,1.4,8.31,2.2,13.18c0.22,1.33,0.43,2.62,0.63,3.8""/>
			<path id=""kvg:08a71-s6"" kvg:type=""㇕b"" d=""M19.64,73.54c6.67-1.14,13.31-2.75,19.46-3.66c1.85-0.27,2.96,1.26,2.7,2.51c-0.89,4.19-2.46,8.16-4.05,14.07""/>
			<path id=""kvg:08a71-s7"" kvg:type=""㇐b"" d=""M22.49,89.55c4.76-0.55,8.86-1.17,14.02-1.72c0.93-0.1,1.91,0.04,2.97-0.09""/>
		</g>
	</g>
	<g id=""kvg:08a71-g3"" kvg:element=""舌"" kvg:position=""right"" kvg:phon=""舌"">
		<g id=""kvg:08a71-g4"" kvg:element=""千"" kvg:position=""top"">
			<path id=""kvg:08a71-s8"" kvg:type=""㇒"" d=""M81.75,13.75c-0.12,1.25-0.79,2.39-1.66,3.06c-4.84,3.69-15.34,9.94-29.34,14.94""/>
			<path id=""kvg:08a71-s9"" kvg:type=""㇐"" d=""M45.39,45.8c1.55,0.34,4.36,0.62,6.73,0.34C62.8,44.9,76.37,43.07,91,42.23c2.28-0.13,4.49-0.01,6.75,0.29""/>
			<path id=""kvg:08a71-s10"" kvg:type=""㇑a"" d=""M68.87,27.19c1.4,1.4,1.85,3.06,1.85,4.88c0,1.44-0.21,28.18-0.21,35.16""/>
		</g>
		<g id=""kvg:08a71-g5"" kvg:element=""口"" kvg:position=""bottom"">
			<path id=""kvg:08a71-s11"" kvg:type=""㇑"" d=""M52.07,67.83c0.93,1.17,1.22,1.76,1.35,2.34c1.38,6.2,2.13,12.33,3.08,20.33c0.13,1.14,0.27,2.31,0.42,3.53""/>
			<path id=""kvg:08a71-s12"" kvg:type=""㇕b"" d=""M54.62,69.29c12.9-1.37,25.41-2.73,32.68-3.31c2.94-0.24,3.79,2.4,3.37,3.8c-1.47,4.87-3.03,11.64-5.04,18.35""/>
			<path id=""kvg:08a71-s13"" kvg:type=""㇐b"" d=""M56.64,91.74c7.5-0.57,16.99-1.13,27.14-1.93c1.37-0.11,2.75-0.22,4.13-0.34""/>
		</g>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_08a71"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 17.50 14.63)"">1</text>
	<text transform=""matrix(1 0 0 1 3.50 34.63)"">2</text>
	<text transform=""matrix(1 0 0 1 10.67 48.13)"">3</text>
	<text transform=""matrix(1 0 0 1 10.50 61.63)"">4</text>
	<text transform=""matrix(1 0 0 1 10.68 78.50)"">5</text>
	<text transform=""matrix(1 0 0 1 20.50 70.63)"">6</text>
	<text transform=""matrix(1 0 0 1 25.50 85.50)"">7</text>
	<text transform=""matrix(1 0 0 1 72.50 13.50)"">8</text>
	<text transform=""matrix(1 0 0 1 47.25 43.50)"">9</text>
	<text transform=""matrix(1 0 0 1 59.50 36.50)"">10</text>
	<text transform=""matrix(1 0 0 1 45.50 78.50)"">11</text>
	<text transform=""matrix(1 0 0 1 53.50 66.50)"">12</text>
	<text transform=""matrix(1 0 0 1 60.50 88.50)"">13</text>
</g>
</svg>
",
                OrderIndex = 9,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
        };

        foreach (var item in kanjiItems6)
        {
            if (!await db.KanjiItems.AnyAsync(k => k.Character == item.Character))
                db.KanjiItems.Add(item);
        }

        var vocabItems6 = new List<KanjiVocabulary>
        {
            new KanjiVocabulary {
                Id = Guid.Parse("d5f99b7b-0954-5eb1-a8d9-1950720ba379"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("f0bb2995-c00a-5312-bf0c-dad7fdc550e7"),
                Level = "N3",
                Word = "今年",
                Reading = "ことし",
                Meaning = "năm nay",
                OrderIndex = 1,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("28f98604-2d1e-50ce-801c-31b6662b141a"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("f0bb2995-c00a-5312-bf0c-dad7fdc550e7"),
                Level = "N3",
                Word = "今日",
                Reading = "きょう",
                Meaning = "hôm nay",
                OrderIndex = 2,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("e1682384-80fb-5201-93fc-e319c19ec606"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("f0bb2995-c00a-5312-bf0c-dad7fdc550e7"),
                Level = "N3",
                Word = "今月",
                Reading = "こんげつ",
                Meaning = "tháng này",
                OrderIndex = 3,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("577ffdf1-a054-5f93-8229-bc69673ebbc4"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("f0bb2995-c00a-5312-bf0c-dad7fdc550e7"),
                Level = "N3",
                Word = "今週",
                Reading = "こんしゅう",
                Meaning = "tuần này",
                OrderIndex = 4,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("17a4a42f-7bbb-5a1a-8f97-91d66ca983b2"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("e2ddd4ff-b156-54da-b717-94b9ea0625bc"),
                Level = "N3",
                Word = "来ます",
                Reading = "きます",
                Meaning = "đến",
                OrderIndex = 5,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("ad20e938-b710-56e8-b870-d3b235bf506d"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("e2ddd4ff-b156-54da-b717-94b9ea0625bc"),
                Level = "N3",
                Word = "来週",
                Reading = "らいしゅう",
                Meaning = "tuần sau",
                OrderIndex = 6,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("ebd4c716-5b92-504f-88f1-91900b816fb4"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("e2ddd4ff-b156-54da-b717-94b9ea0625bc"),
                Level = "N3",
                Word = "来月",
                Reading = "らいげつ",
                Meaning = "tháng sau",
                OrderIndex = 7,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("35cb15c5-f3fb-50f5-9eaf-1351200ca131"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("e2ddd4ff-b156-54da-b717-94b9ea0625bc"),
                Level = "N3",
                Word = "来年",
                Reading = "らいねん",
                Meaning = "năm sau",
                OrderIndex = 8,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("120d8005-61e0-586f-b1bf-ccaa8af2a087"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("e2ddd4ff-b156-54da-b717-94b9ea0625bc"),
                Level = "N3",
                Word = "来日",
                Reading = "らいにち",
                Meaning = "đến Nhật Bản",
                OrderIndex = 9,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("95f75d8b-ffe7-5f6e-9b07-6e247859dafc"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("ccd5f9fb-158b-58ea-9717-1806bf5c0d89"),
                Level = "N3",
                Word = "帰ります",
                Reading = "かえります",
                Meaning = "về",
                OrderIndex = 10,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("19bd0255-1509-5ab0-bfe5-5db20932f0f3"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("ccd5f9fb-158b-58ea-9717-1806bf5c0d89"),
                Level = "N3",
                Word = "日帰り",
                Reading = "ひがえり",
                Meaning = "đi về trong ngày",
                OrderIndex = 11,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("2e1f4c0c-02dc-578b-9f4e-6108e0ba0103"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("ccd5f9fb-158b-58ea-9717-1806bf5c0d89"),
                Level = "N3",
                Word = "帰国",
                Reading = "きこく",
                Meaning = "về nước",
                OrderIndex = 12,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("95a273a5-ae9f-5fa8-b0c8-3fbfaedcd3cb"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("354fb615-4b3c-5370-bd15-bcaa1c1b81bc"),
                Level = "N3",
                Word = "会います",
                Reading = "あいます",
                Meaning = "gặp",
                OrderIndex = 13,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("44339486-acbb-53fb-b606-aee358b5f7b7"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("354fb615-4b3c-5370-bd15-bcaa1c1b81bc"),
                Level = "N3",
                Word = "飲み会",
                Reading = "のみかい",
                Meaning = "nhậu",
                OrderIndex = 14,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("01ac7cf4-ad81-5a4f-b664-95c660153162"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("354fb615-4b3c-5370-bd15-bcaa1c1b81bc"),
                Level = "N3",
                Word = "国会",
                Reading = "こっかい",
                Meaning = "quốc hội",
                OrderIndex = 15,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("431299c9-d53a-5405-a7b6-4d0540244269"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("354fb615-4b3c-5370-bd15-bcaa1c1b81bc"),
                Level = "N3",
                Word = "会見",
                Reading = "かいけん",
                Meaning = "họp báo",
                OrderIndex = 16,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("4498c872-603e-5771-98b3-9d3d7d88ed05"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("354fb615-4b3c-5370-bd15-bcaa1c1b81bc"),
                Level = "N3",
                Word = "会社",
                Reading = "かいしゃ",
                Meaning = "công ty",
                OrderIndex = 17,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("684af827-188c-55bc-8bb3-06993bbbebf8"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("354fb615-4b3c-5370-bd15-bcaa1c1b81bc"),
                Level = "N3",
                Word = "社会",
                Reading = "しゃかい",
                Meaning = "xã hội",
                OrderIndex = 18,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("8e5e0456-8177-5310-b715-ffd794885866"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("825fb13d-2b5f-5a47-aefc-0fe1c0c3c290"),
                Level = "N3",
                Word = "聞きます",
                Reading = "ききます",
                Meaning = "nghe",
                OrderIndex = 19,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("d69ba01b-66d4-5e9c-b96b-e8f4e41a55df"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("825fb13d-2b5f-5a47-aefc-0fe1c0c3c290"),
                Level = "N3",
                Word = "新聞",
                Reading = "しんぶん",
                Meaning = "báo",
                OrderIndex = 20,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("a10ecc51-a7b4-5991-99e3-f5a3f891c427"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("2d9ed815-cb0f-5465-9d86-9c9c69412804"),
                Level = "N3",
                Word = "読みます",
                Reading = "よみます",
                Meaning = "đọc",
                OrderIndex = 21,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("ddc1c91e-d3ab-5261-8483-de9e6f97c6fe"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("2d9ed815-cb0f-5465-9d86-9c9c69412804"),
                Level = "N3",
                Word = "読み物",
                Reading = "よみもの",
                Meaning = "tài liệu đọc",
                OrderIndex = 22,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("d708e91d-6027-5c0a-a795-8c3b1606c16a"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("af7bb69e-c0ff-5d82-b2fc-ace3112451cb"),
                Level = "N3",
                Word = "書きます",
                Reading = "かきます",
                Meaning = "viết",
                OrderIndex = 23,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("8f88ed0a-9d49-5eaa-86e0-081a40045d83"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("2d9ed815-cb0f-5465-9d86-9c9c69412804"),
                Level = "N3",
                Word = "読書",
                Reading = "どくしょ",
                Meaning = "đọc sách",
                OrderIndex = 24,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("96d406a9-4355-5202-8d95-0131da1d476d"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("823b9d3b-c02f-5bab-bba7-12f8c7751748"),
                Level = "N3",
                Word = "話します",
                Reading = "はなします",
                Meaning = "nói chuyện",
                OrderIndex = 25,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("d833c29f-266a-54a4-8b8f-085701a1ee6c"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("823b9d3b-c02f-5bab-bba7-12f8c7751748"),
                Level = "N3",
                Word = "話",
                Reading = "はなし",
                Meaning = "câu chuyện",
                OrderIndex = 26,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("06e2ae88-d9da-521c-94ec-db937c526e0b"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("354fb615-4b3c-5370-bd15-bcaa1c1b81bc"),
                Level = "N3",
                Word = "会話",
                Reading = "かいわ",
                Meaning = "hội thoại",
                OrderIndex = 27,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("27486b52-1052-575e-83c3-ff155c62d370"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("823b9d3b-c02f-5bab-bba7-12f8c7751748"),
                Level = "N3",
                Word = "電話",
                Reading = "でんわ",
                Meaning = "điện thoại",
                OrderIndex = 28,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("89f112fa-8a16-586f-937f-c1afebb8ac10"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("f0bb2995-c00a-5312-bf0c-dad7fdc550e7"),
                Level = "N3",
                Word = "今",
                Reading = "いま",
                Meaning = "bây giờ",
                OrderIndex = 29,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("8cfa6207-6423-593b-8952-d474bb460f7a"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("af7bb69e-c0ff-5d82-b2fc-ace3112451cb"),
                Level = "N3",
                Word = "辞書",
                Reading = "じしょ",
                Meaning = "từ điển",
                OrderIndex = 30,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("f484cb8e-47cb-540f-b9cd-6345870e9710"),
                LessonId = lesson6Id,
                KanjiItemId = Guid.Parse("af7bb69e-c0ff-5d82-b2fc-ace3112451cb"),
                Level = "N3",
                Word = "書き物",
                Reading = "かきもの",
                Meaning = "tài liệu viết",
                OrderIndex = 31,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("8750a887-7003-5116-9307-e7b1831f2223"),
                LessonId = lesson6Id,
                KanjiItemId = null,
                Level = "N3",
                Word = "お寺",
                Reading = "おてら",
                Meaning = "chùa",
                OrderIndex = 32,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("cec15d05-191f-56cd-8dd3-9a34b686e38e"),
                LessonId = lesson6Id,
                KanjiItemId = null,
                Level = "N3",
                Word = "言います",
                Reading = "いいます",
                Meaning = "nói",
                OrderIndex = 33,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("f65d653b-6569-5e57-acfc-5d55b45970f4"),
                LessonId = lesson6Id,
                KanjiItemId = null,
                Level = "N3",
                Word = "言語",
                Reading = "げんご",
                Meaning = "ngôn ngữ",
                OrderIndex = 34,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("f634a252-2cdd-5223-9912-b7eabac350ec"),
                LessonId = lesson6Id,
                KanjiItemId = null,
                Level = "N3",
                Word = "言葉",
                Reading = "ことば",
                Meaning = "từ vựng",
                OrderIndex = 35,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("a26981a8-3bab-54f3-b037-dd2a9901f98c"),
                LessonId = lesson6Id,
                KanjiItemId = null,
                Level = "N3",
                Word = "貝",
                Reading = "かい",
                Meaning = "con sò",
                OrderIndex = 36,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("f6063b99-ddf0-53d0-9a59-4fdf8ee28a9f"),
                LessonId = lesson6Id,
                KanjiItemId = null,
                Level = "N3",
                Word = "田んぼ",
                Reading = "たんぼ",
                Meaning = "cánh đồng",
                OrderIndex = 37,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("a6ec786e-4d32-546d-8958-cb7b461e50fb"),
                LessonId = lesson6Id,
                KanjiItemId = null,
                Level = "N3",
                Word = "水田",
                Reading = "すいでん",
                Meaning = "cánh đồng lúa nước",
                OrderIndex = 38,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("76ed4f2f-d79c-5cbc-ac02-0aca47ecec8c"),
                LessonId = lesson6Id,
                KanjiItemId = null,
                Level = "N3",
                Word = "力",
                Reading = "ちから",
                Meaning = "sức lực",
                OrderIndex = 39,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("d064e974-eb10-5578-b912-03aa378b5be2"),
                LessonId = lesson6Id,
                KanjiItemId = null,
                Level = "N3",
                Word = "水力",
                Reading = "すいりょく",
                Meaning = "sức nước",
                OrderIndex = 40,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("b80bec60-a1b4-542d-949d-00f1cafc1b4a"),
                LessonId = lesson6Id,
                KanjiItemId = null,
                Level = "N3",
                Word = "火力",
                Reading = "かりょく",
                Meaning = "công suất nhiệt, hỏa lực",
                OrderIndex = 41,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("588460ef-4f5c-5b9f-947c-9d928e190932"),
                LessonId = lesson6Id,
                KanjiItemId = null,
                Level = "N3",
                Word = "門",
                Reading = "もん",
                Meaning = "cổng, cửa",
                OrderIndex = 42,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
        };

        foreach (var item in vocabItems6)
        {
            if (!await db.KanjiVocabularyItems.AnyAsync(v => v.Word == item.Word && v.Reading == item.Reading))
                db.KanjiVocabularyItems.Add(item);
        }

        // Lesson 7: Tự nhiên và Cơ bản (JPD123)
        var lesson7Id = Guid.Parse("870a45b6-b242-5803-a0e5-35effe8ece29");
        if (!await db.KanjiLessons.AnyAsync(l => l.Id == lesson7Id))
        {
            db.KanjiLessons.Add(new KanjiLesson { Id = lesson7Id, Level = "N3", LessonNumber = 7, Title = "Tự nhiên và Cơ bản", Description = "Tự nhiên và Cơ bản - JPD123 Kanji N3.", AccessTier = "free", PackageCode = "kanji_jpd123", OrderIndex = 7, CreatedAt = SeededAt, UpdatedAt = SeededAt });
        }

        var kanjiItems7 = new List<KanjiItem>
        {
            new KanjiItem {
                Id = Guid.Parse("06718312-d179-57e1-846a-4c94b7492c57"),
                LessonId = lesson7Id,
                Level = "N3",
                Character = "寺",
                HanViet = "TỰ",
                Meaning = "Chùa",
                StrokeCount = 6,
                KunReading = "てら",
                OnReading = "ジ",
                Mnemonic = "Dùng tay (寸) đắp từng tấc đất (土) để xây dựng ngôi chùa (寺).",
                ComponentMapJson = @"[{""character"": ""寸"", ""component"": ""寸"", ""name"": ""寸"", ""meaning"": ""寸""}, {""character"": ""土"", ""component"": ""土"", ""name"": ""土"", ""meaning"": ""土""}]",
                StrokeDataJson = @"[""M29.88,28.28c1.66,0.31,3.87,0.6,6.37,0.31c10.23-1.18,24.38-2.59,37.19-3.31c2.76-0.16,4.43,0.15,5.82,0.3"", ""M52.55,10.37c1.2,1.13,1.82,2.62,1.82,4.6c0,8.45,0.13,26.9,0.13,27.79"", ""M13.38,45.54c2.71,0.64,7.69,0.85,10.39,0.64c20.86-1.56,45.36-3.47,61.38-3.94c4.51-0.13,7.22,0.31,9.48,0.63"", ""M21.13,64.99c1.9,0.46,5.39,0.54,7.3,0.46c16.2-0.7,40.2-3.2,55.42-3.64c3.17-0.09,5.07,0.22,6.66,0.44"", ""M66.07,49.33c0.99,0.99,1.65,2.79,1.65,4.81c0,12.04-0.15,34.92-0.15,39.02c0,9.83-5.96,1.47-7.96,0.21"", ""M36,74.25c3.18,1.9,8.21,7.8,9,10.75""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_05bfa"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:05bfa"" kvg:element=""寺"">
	<g id=""kvg:05bfa-g1"" kvg:element=""土"" kvg:position=""top"" kvg:radical=""nelson"" kvg:phon=""之V"">
		<path id=""kvg:05bfa-s1"" kvg:type=""㇐"" d=""M29.88,28.28c1.66,0.31,3.87,0.6,6.37,0.31c10.23-1.18,24.38-2.59,37.19-3.31c2.76-0.16,4.43,0.15,5.82,0.3""/>
		<path id=""kvg:05bfa-s2"" kvg:type=""㇑a"" d=""M52.55,10.37c1.2,1.13,1.82,2.62,1.82,4.6c0,8.45,0.13,26.9,0.13,27.79""/>
		<path id=""kvg:05bfa-s3"" kvg:type=""㇐"" d=""M13.38,45.54c2.71,0.64,7.69,0.85,10.39,0.64c20.86-1.56,45.36-3.47,61.38-3.94c4.51-0.13,7.22,0.31,9.48,0.63""/>
	</g>
	<g id=""kvg:05bfa-g2"" kvg:element=""寸"" kvg:position=""bottom"" kvg:radical=""tradit"">
		<path id=""kvg:05bfa-s4"" kvg:type=""㇐"" d=""M21.13,64.99c1.9,0.46,5.39,0.54,7.3,0.46c16.2-0.7,40.2-3.2,55.42-3.64c3.17-0.09,5.07,0.22,6.66,0.44""/>
		<path id=""kvg:05bfa-s5"" kvg:type=""㇚"" d=""M66.07,49.33c0.99,0.99,1.65,2.79,1.65,4.81c0,12.04-0.15,34.92-0.15,39.02c0,9.83-5.96,1.47-7.96,0.21""/>
		<path id=""kvg:05bfa-s6"" kvg:type=""㇔"" d=""M36,74.25c3.18,1.9,8.21,7.8,9,10.75""/>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_05bfa"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 23.50 29.50)"">1</text>
	<text transform=""matrix(1 0 0 1 42.50 10.50)"">2</text>
	<text transform=""matrix(1 0 0 1 5.25 46.63)"">3</text>
	<text transform=""matrix(1 0 0 1 12.75 66.13)"">4</text>
	<text transform=""matrix(1 0 0 1 57.75 55.63)"">5</text>
	<text transform=""matrix(1 0 0 1 27.75 78.13)"">6</text>
</g>
</svg>
",
                OrderIndex = 1,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("647be324-4dfe-51af-994b-df33182c6398"),
                LessonId = lesson7Id,
                Level = "N3",
                Character = "言",
                HanViet = "NGÔN",
                Meaning = "Nói, ngôn từ",
                StrokeCount = 7,
                KunReading = "い.う、こと",
                OnReading = "ゴン、ゲン",
                Mnemonic = "Kẻ cầm đầu (亠) có đến hai (二) cái miệng (口) để thay đổi ngôn từ lắt léo.",
                ComponentMapJson = @"[{""character"": ""亠"", ""component"": ""亠"", ""name"": ""亠"", ""meaning"": ""亠""}, {""character"": ""二"", ""component"": ""二"", ""name"": ""二"", ""meaning"": ""二""}, {""character"": ""口"", ""component"": ""口"", ""name"": ""口"", ""meaning"": ""口""}]",
                StrokeDataJson = @"[""M48.38,11.25c4.38,2.5,8.88,7.75,10.38,11.5"", ""M14.88,33.98c2.52,0.54,6.91,0.76,9.42,0.54c22.95-2.02,40.82-4.02,59.99-4.73c4.2-0.16,6.73,0.26,8.83,0.53"", ""M38.63,46.65C40,47,41,47,42.45,46.88c7.06-0.6,18.6-2.27,22.81-2.6c1.86-0.15,3.36-0.15,4.74,0.22"", ""M37.88,61.4c1.5,0.23,2.75,0.35,4.16,0.23c7.68-0.67,20.23-2.28,24.8-2.85c2.16-0.27,3.66-0.15,5.17,0.22"", ""M37,74.75c0.81,0.81,1.4,1.76,1.53,2.77c0.85,6.73,1.9,11.43,2.89,18.45c0.18,1.24,0.35,2.43,0.54,3.53"", ""M39.53,76.74c9.24-1.7,22.59-3.37,30.29-4.25c2.21-0.25,3.55,1.17,3.24,2.32c-0.69,2.52-3.74,12.7-4.94,16.98"", ""M42.2,95.16c6.19-0.53,16.55-1.39,25.32-2.22c1.33-0.13,2.73-0.12,3.95-0.12""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_08a00"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:08a00"" kvg:element=""言"" kvg:radical=""general"">
	<path id=""kvg:08a00-s1"" kvg:type=""㇔"" d=""M48.38,11.25c4.38,2.5,8.88,7.75,10.38,11.5""/>
	<path id=""kvg:08a00-s2"" kvg:type=""㇐"" d=""M14.88,33.98c2.52,0.54,6.91,0.76,9.42,0.54c22.95-2.02,40.82-4.02,59.99-4.73c4.2-0.16,6.73,0.26,8.83,0.53""/>
	<path id=""kvg:08a00-s3"" kvg:type=""㇐"" d=""M38.63,46.65C40,47,41,47,42.45,46.88c7.06-0.6,18.6-2.27,22.81-2.6c1.86-0.15,3.36-0.15,4.74,0.22""/>
	<path id=""kvg:08a00-s4"" kvg:type=""㇐"" d=""M37.88,61.4c1.5,0.23,2.75,0.35,4.16,0.23c7.68-0.67,20.23-2.28,24.8-2.85c2.16-0.27,3.66-0.15,5.17,0.22""/>
	<g id=""kvg:08a00-g1"" kvg:element=""口"">
		<path id=""kvg:08a00-s5"" kvg:type=""㇑"" d=""M37,74.75c0.81,0.81,1.4,1.76,1.53,2.77c0.85,6.73,1.9,11.43,2.89,18.45c0.18,1.24,0.35,2.43,0.54,3.53""/>
		<path id=""kvg:08a00-s6"" kvg:type=""㇕b"" d=""M39.53,76.74c9.24-1.7,22.59-3.37,30.29-4.25c2.21-0.25,3.55,1.17,3.24,2.32c-0.69,2.52-3.74,12.7-4.94,16.98""/>
		<path id=""kvg:08a00-s7"" kvg:type=""㇐b"" d=""M42.2,95.16c6.19-0.53,16.55-1.39,25.32-2.22c1.33-0.13,2.73-0.12,3.95-0.12""/>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_08a00"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 41.25 12.13)"">1</text>
	<text transform=""matrix(1 0 0 1 6.75 36.13)"">2</text>
	<text transform=""matrix(1 0 0 1 30.75 46.63)"">3</text>
	<text transform=""matrix(1 0 0 1 29.25 63.13)"">4</text>
	<text transform=""matrix(1 0 0 1 30.50 82.63)"">5</text>
	<text transform=""matrix(1 0 0 1 39.50 73.63)"">6</text>
	<text transform=""matrix(1 0 0 1 44.50 91.63)"">7</text>
</g>
</svg>
",
                OrderIndex = 2,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("15c53e95-a929-5d5a-beb2-b1332ad72428"),
                LessonId = lesson7Id,
                Level = "N3",
                Character = "貝",
                HanViet = "BỐI",
                Meaning = "Vỏ sò, tiền tệ",
                StrokeCount = 7,
                KunReading = "かい",
                OnReading = "バイ",
                Mnemonic = "Con sò (貝) mở mắt (目) to và thò hai cái chân (ハ) ra ngoài để đi tìm tiền.",
                ComponentMapJson = @"[{""character"": ""目"", ""component"": ""目"", ""name"": ""目"", ""meaning"": ""目""}]",
                StrokeDataJson = @"[""M31.75,17.8c1.16,1.16,1.68,2.84,1.68,4.63c0,1.49-0.18,33.31-0.18,48.31c0,3.5-0.07,3.62-0.07,6.5"", ""M33.82,19.85c7.3-0.73,30.8-3.48,37.67-4.11c2.94-0.27,4.51,1,4.51,3.66c0,3.09-0.5,32.69-0.74,47.84c-0.08,4.93-0.13,8.28-0.13,8.6"", ""M34.76,38.26c8.99-1.26,33.57-3.64,39.78-3.64"", ""M34.82,55.24c10.93-0.99,27.93-2.99,39.51-3.18"", ""M34.57,73.74C45.5,73,64.12,71.05,74.08,71.05"", ""M41.95,80.5c0.55,1.41-0.42,3.3-1.5,4.33C37.17,87.94,28.56,94.07,22,97.75"", ""M65.75,80C72.78,84.75,81.08,93.35,83,97.5""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_08c9d"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:08c9d"" kvg:element=""貝"" kvg:radical=""general"">
	<g id=""kvg:08c9d-g1"" kvg:element=""目"" kvg:position=""top"">
		<path id=""kvg:08c9d-s1"" kvg:type=""㇑"" d=""M31.75,17.8c1.16,1.16,1.68,2.84,1.68,4.63c0,1.49-0.18,33.31-0.18,48.31c0,3.5-0.07,3.62-0.07,6.5""/>
		<path id=""kvg:08c9d-s2"" kvg:type=""㇕a"" d=""M33.82,19.85c7.3-0.73,30.8-3.48,37.67-4.11c2.94-0.27,4.51,1,4.51,3.66c0,3.09-0.5,32.69-0.74,47.84c-0.08,4.93-0.13,8.28-0.13,8.6""/>
		<path id=""kvg:08c9d-s3"" kvg:type=""㇐a"" d=""M34.76,38.26c8.99-1.26,33.57-3.64,39.78-3.64""/>
		<path id=""kvg:08c9d-s4"" kvg:type=""㇐a"" d=""M34.82,55.24c10.93-0.99,27.93-2.99,39.51-3.18""/>
		<path id=""kvg:08c9d-s5"" kvg:type=""㇐a"" d=""M34.57,73.74C45.5,73,64.12,71.05,74.08,71.05""/>
	</g>
	<g id=""kvg:08c9d-g2"" kvg:element=""八"" kvg:position=""bottom"">
		<path id=""kvg:08c9d-s6"" kvg:type=""㇒"" d=""M41.95,80.5c0.55,1.41-0.42,3.3-1.5,4.33C37.17,87.94,28.56,94.07,22,97.75""/>
		<path id=""kvg:08c9d-s7"" kvg:type=""㇔"" d=""M65.75,80C72.78,84.75,81.08,93.35,83,97.5""/>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_08c9d"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 25.25 26.63)"">1</text>
	<text transform=""matrix(1 0 0 1 36.25 15.63)"">2</text>
	<text transform=""matrix(1 0 0 1 38.75 34.13)"">3</text>
	<text transform=""matrix(1 0 0 1 39.25 50.63)"">4</text>
	<text transform=""matrix(1 0 0 1 39.25 70.13)"">5</text>
	<text transform=""matrix(1 0 0 1 30.25 84.63)"">6</text>
	<text transform=""matrix(1 0 0 1 57.75 84.13)"">7</text>
</g>
</svg>
",
                OrderIndex = 3,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("9e44fc27-4fc9-5721-96dd-c202def83d6b"),
                LessonId = lesson7Id,
                Level = "N3",
                Character = "田",
                HanViet = "ĐIỀN",
                Meaning = "Ruộng",
                StrokeCount = 5,
                KunReading = "た",
                OnReading = "デン",
                Mnemonic = "Khu đất (囗) được chia thành hình chữ thập (十) để làm bờ ruộng (田).",
                ComponentMapJson = @"[{""character"": ""囗"", ""component"": ""囗"", ""name"": ""囗"", ""meaning"": ""囗""}, {""character"": ""十"", ""component"": ""十"", ""name"": ""十"", ""meaning"": ""十""}]",
                StrokeDataJson = @"[""M18.25,27.48c1.2,1.2,2.11,2.68,2.28,3.95C22,42.25,23.52,62.11,24.81,80c0.17,2.4,0.34,3.75,0.49,6"", ""M20.85,29.29c8.52-0.54,50.04-3.17,61.58-4.05c4.84-0.37,7.32,2.01,7.04,5.63c-0.69,8.8-2.83,30.69-3.99,46.64c-0.18,2.44-0.34,4.75-0.47,6.86"", ""M53.25,29c0.88,0.88,1.19,2.12,1.19,3.5c0,9.52,0.31,46.37,0.31,47.25"", ""M24,55.5c5.75-0.5,55.12-3.25,62.5-3.5"", ""M25.98,83.07c14.77-0.82,39.39-2.07,58.18-2.83""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_07530"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:07530"" kvg:element=""田"" kvg:radical=""general"">
	<path id=""kvg:07530-s1"" kvg:type=""㇑"" d=""M18.25,27.48c1.2,1.2,2.11,2.68,2.28,3.95C22,42.25,23.52,62.11,24.81,80c0.17,2.4,0.34,3.75,0.49,6""/>
	<path id=""kvg:07530-s2"" kvg:type=""㇕a"" d=""M20.85,29.29c8.52-0.54,50.04-3.17,61.58-4.05c4.84-0.37,7.32,2.01,7.04,5.63c-0.69,8.8-2.83,30.69-3.99,46.64c-0.18,2.44-0.34,4.75-0.47,6.86""/>
	<path id=""kvg:07530-s3"" kvg:type=""㇑a"" d=""M53.25,29c0.88,0.88,1.19,2.12,1.19,3.5c0,9.52,0.31,46.37,0.31,47.25""/>
	<path id=""kvg:07530-s4"" kvg:type=""㇐a"" d=""M24,55.5c5.75-0.5,55.12-3.25,62.5-3.5""/>
	<path id=""kvg:07530-s5"" kvg:type=""㇐a"" d=""M25.98,83.07c14.77-0.82,39.39-2.07,58.18-2.83""/>
</g>
</g>
<g id=""kvg:StrokeNumbers_07530"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 11.50 34.63)"">1</text>
	<text transform=""matrix(1 0 0 1 23.50 25.50)"">2</text>
	<text transform=""matrix(1 0 0 1 46.50 37.50)"">3</text>
	<text transform=""matrix(1 0 0 1 27.50 52.50)"">4</text>
	<text transform=""matrix(1 0 0 1 28.50 79.50)"">5</text>
</g>
</svg>
",
                OrderIndex = 4,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("9265dce0-4e5e-5a3c-8441-dfe88d5d9cab"),
                LessonId = lesson7Id,
                Level = "N3",
                Character = "力",
                HanViet = "LỰC",
                Meaning = "Sức lực",
                StrokeCount = 2,
                KunReading = "ちから",
                OnReading = "リョク、リキ",
                Mnemonic = "Chữ này nhìn giống hệt hình ảnh cánh tay đang gồng lên cuộn bắp chuột để khoe sức lực (力).",
                ComponentMapJson = @"[]",
                StrokeDataJson = @"[""M21.5,38.25c2.71,1.16,5.48,1.13,8.27,0.71c14.44-2.14,39.36-6.21,53.23-8.21c4.48-0.65,6.25,1.38,5.5,5.75c-2.81,16.37-9,38.75-20,53.25c-6.14,8.09-9.5,3-12.5-0.75"", ""M56.88,13.68c0.62,2.57,0.56,4.63,0,7.19c-4.11,18.97-16.53,49.27-43.49,68.79""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_0529b"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:0529b"" kvg:element=""力"" kvg:radical=""general"">
	<path id=""kvg:0529b-s1"" kvg:type=""㇆"" d=""M21.5,38.25c2.71,1.16,5.48,1.13,8.27,0.71c14.44-2.14,39.36-6.21,53.23-8.21c4.48-0.65,6.25,1.38,5.5,5.75c-2.81,16.37-9,38.75-20,53.25c-6.14,8.09-9.5,3-12.5-0.75""/>
	<path id=""kvg:0529b-s2"" kvg:type=""㇒"" d=""M56.88,13.68c0.62,2.57,0.56,4.63,0,7.19c-4.11,18.97-16.53,49.27-43.49,68.79""/>
</g>
</g>
<g id=""kvg:StrokeNumbers_0529b"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 14.75 38.63)"">1</text>
	<text transform=""matrix(1 0 0 1 46.75 12.13)"">2</text>
</g>
</svg>
",
                OrderIndex = 5,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("c6ebc2d7-7d08-5e65-a953-9924c7161f0c"),
                LessonId = lesson7Id,
                Level = "N3",
                Character = "門",
                HanViet = "MÔN",
                Meaning = "Cửa, cổng",
                StrokeCount = 8,
                KunReading = "かど",
                OnReading = "モン",
                Mnemonic = "Hình ảnh hai cánh cổng (門) lớn đang được mở tung ra hai bên.",
                ComponentMapJson = @"[]",
                StrokeDataJson = @"[""M17.39,14.97c0.94,0.94,1.26,2.28,1.26,4c0,0.77-0.18,49.78-0.18,69.28c0,3.3-0.04,4.11-0.07,4.97"", ""M19.77,17.18c6.04-0.81,20.02-2.86,21.83-2.97c1.91-0.12,2.9,1.04,3,1.96c0.11,1.07-1.17,15.69-1.78,22.84c-0.19,2.21-0.31,3.71-0.31,3.87"", ""M20.06,30.18C27,29,36.5,27.75,42.46,27.02"", ""M19.61,43.82c8.39-1.32,14.14-2.57,21.55-3.17"", ""M61.96,13.47c0.89,1.19,0.92,2.64,0.91,4.04c-0.02,3.82-0.08,14.92-0.05,20.49c0.01,1.84,0.03,3.07,0.06,3.25"", ""M63.98,15.09c6.63-0.9,21.65-3.51,23.46-3.6c1.96-0.1,3.82,1.63,3.82,2.98c0,18.78-0.52,61.53-0.51,75.14c0.01,11.13-5.24,3.63-9.49-0.12"", ""M64.34,26.89c6.16-0.89,20.29-2.39,25.5-2.67"", ""M64.13,39.63c8.99-1,15.87-1.63,25.9-2.47""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_09580"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:09580"" kvg:element=""門"" kvg:radical=""general"">
	<g id=""kvg:09580-g1"" kvg:position=""left"">
		<path id=""kvg:09580-s1"" kvg:type=""㇑"" d=""M17.39,14.97c0.94,0.94,1.26,2.28,1.26,4c0,0.77-0.18,49.78-0.18,69.28c0,3.3-0.04,4.11-0.07,4.97""/>
		<path id=""kvg:09580-s2"" kvg:type=""㇕a"" d=""M19.77,17.18c6.04-0.81,20.02-2.86,21.83-2.97c1.91-0.12,2.9,1.04,3,1.96c0.11,1.07-1.17,15.69-1.78,22.84c-0.19,2.21-0.31,3.71-0.31,3.87""/>
		<path id=""kvg:09580-s3"" kvg:type=""㇐a"" d=""M20.06,30.18C27,29,36.5,27.75,42.46,27.02""/>
		<path id=""kvg:09580-s4"" kvg:type=""㇐a"" d=""M19.61,43.82c8.39-1.32,14.14-2.57,21.55-3.17""/>
	</g>
	<g id=""kvg:09580-g2"" kvg:position=""right"">
		<path id=""kvg:09580-s5"" kvg:type=""㇑"" d=""M61.96,13.47c0.89,1.19,0.92,2.64,0.91,4.04c-0.02,3.82-0.08,14.92-0.05,20.49c0.01,1.84,0.03,3.07,0.06,3.25""/>
		<path id=""kvg:09580-s6"" kvg:type=""㇆a"" d=""M63.98,15.09c6.63-0.9,21.65-3.51,23.46-3.6c1.96-0.1,3.82,1.63,3.82,2.98c0,18.78-0.52,61.53-0.51,75.14c0.01,11.13-5.24,3.63-9.49-0.12""/>
		<path id=""kvg:09580-s7"" kvg:type=""㇐a"" d=""M64.34,26.89c6.16-0.89,20.29-2.39,25.5-2.67""/>
		<path id=""kvg:09580-s8"" kvg:type=""㇐a"" d=""M64.13,39.63c8.99-1,15.87-1.63,25.9-2.47""/>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_09580"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 9.50 22.50)"">1</text>
	<text transform=""matrix(1 0 0 1 20.50 13.50)"">2</text>
	<text transform=""matrix(1 0 0 1 22.50 26.50)"">3</text>
	<text transform=""matrix(1 0 0 1 22.50 39.50)"">4</text>
	<text transform=""matrix(1 0 0 1 53.50 19.50)"">5</text>
	<text transform=""matrix(1 0 0 1 63.50 11.50)"">6</text>
	<text transform=""matrix(1 0 0 1 66.50 23.50)"">7</text>
	<text transform=""matrix(1 0 0 1 66.50 35.50)"">8</text>
</g>
</svg>
",
                OrderIndex = 6,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("465ad202-c37c-571a-985c-69c96c90a421"),
                LessonId = lesson7Id,
                Level = "N3",
                Character = "肉",
                HanViet = "NHỤC",
                Meaning = "Thịt",
                StrokeCount = 6,
                KunReading = "",
                OnReading = "ニク",
                Mnemonic = "Hai người (人) lén lút trốn vào trong kho lạnh (冂) để lén ăn thịt (肉).",
                ComponentMapJson = @"[{""character"": ""人"", ""component"": ""人"", ""name"": ""人"", ""meaning"": ""人""}, {""character"": ""冂"", ""component"": ""冂"", ""name"": ""冂"", ""meaning"": ""冂""}]",
                StrokeDataJson = @"[""M19,30.27c1.11,1.11,1.44,2.82,1.44,4.56c0,5.46,0.16,39.85,0.21,53.67c0.01,3.04,0.02,5.09,0.02,5.61"", ""M21.01,32.11c16.09-1.31,61.86-4.76,63.38-4.76c3.86,0,5.92,1.9,5.92,5.3c0,1.91,0.24,54.28,0.24,58.68c0,9.67-6.04,2.42-9.44-0.71"", ""M53.28,12.75C54,14,54.03,15.33,54.1,16.79c0.65,13.46-8.35,34.46-24.6,38.71"", ""M54.5,37.25c8.56,3.18,22.11,13.06,24.25,18"", ""M52,51.38c0.12,1.22,0.05,2.34-0.47,3.45c-3.28,7.05-11.03,17.05-21.23,22.4"", ""M52,62.25C60.47,65.03,73.88,73.68,76,78""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_08089"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:08089"" kvg:element=""肉"" kvg:radical=""general"">
	<path id=""kvg:08089-s1"" kvg:type=""㇑"" d=""M19,30.27c1.11,1.11,1.44,2.82,1.44,4.56c0,5.46,0.16,39.85,0.21,53.67c0.01,3.04,0.02,5.09,0.02,5.61""/>
	<path id=""kvg:08089-s2"" kvg:type=""㇆a"" d=""M21.01,32.11c16.09-1.31,61.86-4.76,63.38-4.76c3.86,0,5.92,1.9,5.92,5.3c0,1.91,0.24,54.28,0.24,58.68c0,9.67-6.04,2.42-9.44-0.71""/>
	<path id=""kvg:08089-s3"" kvg:type=""㇒"" d=""M53.28,12.75C54,14,54.03,15.33,54.1,16.79c0.65,13.46-8.35,34.46-24.6,38.71""/>
	<path id=""kvg:08089-s4"" kvg:type=""㇔"" d=""M54.5,37.25c8.56,3.18,22.11,13.06,24.25,18""/>
	<path id=""kvg:08089-s5"" kvg:type=""㇒"" d=""M52,51.38c0.12,1.22,0.05,2.34-0.47,3.45c-3.28,7.05-11.03,17.05-21.23,22.4""/>
	<path id=""kvg:08089-s6"" kvg:type=""㇔"" d=""M52,62.25C60.47,65.03,73.88,73.68,76,78""/>
</g>
</g>
<g id=""kvg:StrokeNumbers_08089"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 13.50 39.50)"">1</text>
	<text transform=""matrix(1 0 0 1 24.50 28.50)"">2</text>
	<text transform=""matrix(1 0 0 1 44.50 14.50)"">3</text>
	<text transform=""matrix(1 0 0 1 61.50 39.13)"">4</text>
	<text transform=""matrix(1 0 0 1 44.50 55.50)"">5</text>
	<text transform=""matrix(1 0 0 1 56.50 62.50)"">6</text>
</g>
</svg>
",
                OrderIndex = 7,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("8dc8753c-940a-55fa-8156-1436615d7e72"),
                LessonId = lesson7Id,
                Level = "N3",
                Character = "料",
                HanViet = "LIỄU",
                Meaning = "Nguyên liệu, tài liệu, phí",
                StrokeCount = 10,
                KunReading = "",
                OnReading = "リョウ",
                Mnemonic = "Dùng cái đấu (斗) để đong lường gạo (米) làm nguyên liệu (料) nấu ăn.",
                ComponentMapJson = @"[{""character"": ""斗"", ""component"": ""斗"", ""name"": ""斗"", ""meaning"": ""斗""}, {""character"": ""米"", ""component"": ""米"", ""name"": ""米"", ""meaning"": ""米""}]",
                StrokeDataJson = @"[""M14.5,24.75c2.99,2.61,7.5,9.99,8.25,14.05"", ""M48.26,19.22c0.08,0.97-0.03,1.92-0.33,2.84c-1.93,4.57-4.3,9.2-8.61,14.49"", ""M12.1,47.8c0.8,0.28,2.95,0.54,5.06,0.28c7.71-0.96,19.07-3.16,27.7-4.05c2.12-0.22,2.66-0.28,4,0"", ""M31.23,13.21c0.84,0.84,1.42,2.04,1.42,3.35c0,0.83,0.05,51.78,0.06,71.44c0,3.47,0,5.96,0,7.04"", ""M31.77,46.71c0,1.41-0.44,2.8-0.85,3.85C26.95,60.89,19.37,72.97,12,79.98"", ""M35.87,54.19c4.7,2.67,7.63,5.56,9.9,10.02"", ""M60,24.25c3.26,1.23,8.43,5.08,9.25,7"", ""M57.25,42c3.35,1.23,8.66,5.08,9.5,7"", ""M53.88,63.96c1.5,0.41,3.69,0.67,5.42,0.27c10.33-2.36,24.08-6.36,33.41-7.45c1.76-0.2,3.43-0.34,4.54,0"", ""M79.62,12.25c0.94,0.94,1.46,2.5,1.46,3.75c0,0.87,0.02,54.18,0.03,74.25c0,3.48,0,5.96,0,7""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_06599"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:06599"" kvg:element=""料"">
	<g id=""kvg:06599-g1"" kvg:element=""米"" kvg:variant=""true"" kvg:position=""left"" kvg:radical=""nelson"">
		<path id=""kvg:06599-s1"" kvg:type=""㇔"" d=""M14.5,24.75c2.99,2.61,7.5,9.99,8.25,14.05""/>
		<path id=""kvg:06599-s2"" kvg:type=""㇒"" d=""M48.26,19.22c0.08,0.97-0.03,1.92-0.33,2.84c-1.93,4.57-4.3,9.2-8.61,14.49""/>
		<path id=""kvg:06599-s3"" kvg:type=""㇐"" d=""M12.1,47.8c0.8,0.28,2.95,0.54,5.06,0.28c7.71-0.96,19.07-3.16,27.7-4.05c2.12-0.22,2.66-0.28,4,0""/>
		<path id=""kvg:06599-s4"" kvg:type=""㇑"" d=""M31.23,13.21c0.84,0.84,1.42,2.04,1.42,3.35c0,0.83,0.05,51.78,0.06,71.44c0,3.47,0,5.96,0,7.04""/>
		<path id=""kvg:06599-s5"" kvg:type=""㇒"" d=""M31.77,46.71c0,1.41-0.44,2.8-0.85,3.85C26.95,60.89,19.37,72.97,12,79.98""/>
		<path id=""kvg:06599-s6"" kvg:type=""㇔/㇏"" d=""M35.87,54.19c4.7,2.67,7.63,5.56,9.9,10.02""/>
	</g>
	<g id=""kvg:06599-g2"" kvg:element=""斗"" kvg:position=""right"" kvg:radical=""tradit"">
		<g id=""kvg:06599-g3"" kvg:element=""丶"">
			<path id=""kvg:06599-s7"" kvg:type=""㇔"" d=""M60,24.25c3.26,1.23,8.43,5.08,9.25,7""/>
		</g>
		<g id=""kvg:06599-g4"" kvg:element=""丶"">
			<path id=""kvg:06599-s8"" kvg:type=""㇔"" d=""M57.25,42c3.35,1.23,8.66,5.08,9.5,7""/>
		</g>
		<path id=""kvg:06599-s9"" kvg:type=""㇐"" d=""M53.88,63.96c1.5,0.41,3.69,0.67,5.42,0.27c10.33-2.36,24.08-6.36,33.41-7.45c1.76-0.2,3.43-0.34,4.54,0""/>
		<path id=""kvg:06599-s10"" kvg:type=""㇑"" d=""M79.62,12.25c0.94,0.94,1.46,2.5,1.46,3.75c0,0.87,0.02,54.18,0.03,74.25c0,3.48,0,5.96,0,7""/>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_06599"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 7.99 23.50)"">1</text>
	<text transform=""matrix(1 0 0 1 40.50 19.50)"">2</text>
	<text transform=""matrix(1 0 0 1 4.50 49.50)"">3</text>
	<text transform=""matrix(1 0 0 1 21.75 13.50)"">4</text>
	<text transform=""matrix(1 0 0 1 20.50 57.50)"">5</text>
	<text transform=""matrix(1 0 0 1 40.50 54.50)"">6</text>
	<text transform=""matrix(1 0 0 1 54.75 23.50)"">7</text>
	<text transform=""matrix(1 0 0 1 54.50 39.50)"">8</text>
	<text transform=""matrix(1 0 0 1 52.50 60.50)"">9</text>
	<text transform=""matrix(1 0 0 1 66.50 12.50)"">10</text>
</g>
</svg>
",
                OrderIndex = 8,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("894bb594-19e0-5f35-8ad3-6b11c0de8351"),
                LessonId = lesson7Id,
                Level = "N3",
                Character = "理",
                HanViet = "LÝ",
                Meaning = "Lý lẽ, logic, địa lý",
                StrokeCount = 11,
                KunReading = "",
                OnReading = "リ",
                Mnemonic = "Vị Vua (王) cai quản các ngôi làng (里) bằng lý (理) lẽ vô cùng công bằng.",
                ComponentMapJson = @"[{""character"": ""王"", ""component"": ""王"", ""name"": ""王"", ""meaning"": ""王""}, {""character"": ""里"", ""component"": ""里"", ""name"": ""里"", ""meaning"": ""里""}]",
                StrokeDataJson = @"[""M11.75,27.75c1.87,0.5,3.23,0.52,5.1,0.25c6.9-1,13.82-2.25,19.42-3.25c1.21-0.22,2.49,0,3.73,0"", ""M26.25,30.25c1,1,1.75,2.5,1.75,3.75s0,40.25,0,43"", ""M15.25,52.09c1.25,0.19,1.98,0.58,4.25,0.19c5.88-1.02,14.39-3.36,16.75-4.09c1-0.31,1.88-0.44,3-0.44"", ""M12.75,82.75c1.25,1,3.3,0.91,4.25,0.5c5.75-2.5,17.25-8,22-10.5"", ""M46,18.5c1.12,1.12,1.78,2.13,2,3.2c1.33,6.47,2.58,18.93,3.5,29.28c0.12,1.32,0.23,2.62,0.34,3.88"", ""M48.79,20.78c12.46-2.03,30.96-4.78,39.73-5.5c3.15-0.26,4.73,1.65,4.57,3.26c-0.48,4.81-2.23,18.91-3.61,28.04c-0.31,2.05-0.6,3.86-0.86,5.25"", ""M51.2,35.59c6.05-0.84,31.05-3.47,39.14-3.53"", ""M52.67,52.28c9.58-0.78,22.08-2.28,35.19-3.02"", ""M67.75,20.75c0.88,0.88,1.41,1.99,1.44,3.01c0.3,12.89,0.12,59.3,0.12,63.99"", ""M52.15,69.29c1.38,0.39,3.91,0.49,5.3,0.39c9.02-0.67,15.56-1.92,24.52-3.17c2.27-0.32,3.66-0.26,6.03-0.26"", ""M40.13,91c1.57,0.54,4.47,0.73,6.04,0.54c16.83-2.05,35.08-4.3,47.45-4.27c2.62,0.01,4.2,0.26,5.51,0.53""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_07406"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:07406"" kvg:element=""理"">
	<g id=""kvg:07406-g1"" kvg:element=""王"" kvg:variant=""true"" kvg:original=""玉"" kvg:partial=""true"" kvg:position=""left"" kvg:radical=""general"">
		<path id=""kvg:07406-s1"" kvg:type=""㇐"" d=""M11.75,27.75c1.87,0.5,3.23,0.52,5.1,0.25c6.9-1,13.82-2.25,19.42-3.25c1.21-0.22,2.49,0,3.73,0""/>
		<path id=""kvg:07406-s2"" kvg:type=""㇑a"" d=""M26.25,30.25c1,1,1.75,2.5,1.75,3.75s0,40.25,0,43""/>
		<path id=""kvg:07406-s3"" kvg:type=""㇐"" d=""M15.25,52.09c1.25,0.19,1.98,0.58,4.25,0.19c5.88-1.02,14.39-3.36,16.75-4.09c1-0.31,1.88-0.44,3-0.44""/>
		<path id=""kvg:07406-s4"" kvg:type=""㇀/㇐"" d=""M12.75,82.75c1.25,1,3.3,0.91,4.25,0.5c5.75-2.5,17.25-8,22-10.5""/>
	</g>
	<g id=""kvg:07406-g2"" kvg:element=""里"" kvg:position=""right"" kvg:phon=""里"">
		<g id=""kvg:07406-g3"" kvg:element=""日"">
			<path id=""kvg:07406-s5"" kvg:type=""㇑"" d=""M46,18.5c1.12,1.12,1.78,2.13,2,3.2c1.33,6.47,2.58,18.93,3.5,29.28c0.12,1.32,0.23,2.62,0.34,3.88""/>
			<path id=""kvg:07406-s6"" kvg:type=""㇕a"" d=""M48.79,20.78c12.46-2.03,30.96-4.78,39.73-5.5c3.15-0.26,4.73,1.65,4.57,3.26c-0.48,4.81-2.23,18.91-3.61,28.04c-0.31,2.05-0.6,3.86-0.86,5.25""/>
			<path id=""kvg:07406-s7"" kvg:type=""㇐a"" d=""M51.2,35.59c6.05-0.84,31.05-3.47,39.14-3.53""/>
			<path id=""kvg:07406-s8"" kvg:type=""㇐a"" d=""M52.67,52.28c9.58-0.78,22.08-2.28,35.19-3.02""/>
		</g>
		<path id=""kvg:07406-s9"" kvg:type=""㇑a"" d=""M67.75,20.75c0.88,0.88,1.41,1.99,1.44,3.01c0.3,12.89,0.12,59.3,0.12,63.99""/>
		<path id=""kvg:07406-s10"" kvg:type=""㇐"" d=""M52.15,69.29c1.38,0.39,3.91,0.49,5.3,0.39c9.02-0.67,15.56-1.92,24.52-3.17c2.27-0.32,3.66-0.26,6.03-0.26""/>
		<path id=""kvg:07406-s11"" kvg:type=""㇐"" d=""M40.13,91c1.57,0.54,4.47,0.73,6.04,0.54c16.83-2.05,35.08-4.3,47.45-4.27c2.62,0.01,4.2,0.26,5.51,0.53""/>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_07406"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 5.50 29.50)"">1</text>
	<text transform=""matrix(1 0 0 1 19.50 39.13)"">2</text>
	<text transform=""matrix(1 0 0 1 7.50 53.50)"">3</text>
	<text transform=""matrix(1 0 0 1 4.50 84.50)"">4</text>
	<text transform=""matrix(1 0 0 1 41.50 33.50)"">5</text>
	<text transform=""matrix(1 0 0 1 50.50 17.50)"">6</text>
	<text transform=""matrix(1 0 0 1 54.75 32.50)"">7</text>
	<text transform=""matrix(1 0 0 1 54.50 49.50)"">8</text>
	<text transform=""matrix(1 0 0 1 62.25 27.50)"">9</text>
	<text transform=""matrix(1 0 0 1 49.50 66.13)"">10</text>
	<text transform=""matrix(1 0 0 1 36.50 87.50)"">11</text>
</g>
</svg>
",
                OrderIndex = 9,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("01b783d3-c933-5a04-aba8-e7af559397e1"),
                LessonId = lesson7Id,
                Level = "N3",
                Character = "野",
                HanViet = "DÃ",
                Meaning = "Cánh đồng, hoang dã",
                StrokeCount = 11,
                KunReading = "の",
                OnReading = "ヤ",
                Mnemonic = "Người trong làng (里) đã dự (予) đoán trước được sẽ có thú hoang dã (野) chạy ra cánh đồng.",
                ComponentMapJson = @"[{""character"": ""里"", ""component"": ""里"", ""name"": ""里"", ""meaning"": ""里""}, {""character"": ""予"", ""component"": ""予"", ""name"": ""予"", ""meaning"": ""予""}]",
                StrokeDataJson = @"[""M12.5,18.54c1.25,0.96,1.45,2.21,1.62,3.18c0.98,5.66,2.66,16.78,3.4,25.76c0.08,1,0.15,1.98,0.21,2.93"", ""M15.03,20.17c10.69-1.68,25.59-3.93,31.58-4.62c2.5-0.29,3.63,1.82,3.4,3.34c-0.89,5.98-2.33,16.31-3.89,25.13c-0.23,1.31-0.45,2.06-0.65,3.21"", ""M16.74,34.44c2.97-0.38,29.08-3.9,31.53-3.98"", ""M17.7,48.43c6.57-0.22,18.7-2.92,27.77-3.37"", ""M30.97,19.87c0.78,1.13,1.06,1.76,1.08,3.13c0.21,12.86,0.06,50.65,0.06,55.01"", ""M15.75,62.75c1.72,0.5,3.79,0.44,4.95,0.25c7.75-1.25,14.9-3.05,23.1-4.25c1.7-0.25,3.83-0.25,5.45,0"", ""M12.25,82.31c1.38,1.19,3.39,1.61,6.09,0.69c5.53-1.87,21.25-7.06,30.16-10"", ""M57.51,16.84c2.11,0.66,4.23,0.51,6.25,0.19c7.36-1.16,19.6-3.31,21.94-3.78c2.6-0.52,3.74,1.71,2.19,3.42c-2.66,2.94-12.79,14.29-14.43,16.39"", ""M63.88,28.5c2.86,1.21,10.28,5.85,11.62,9"", ""M54.42,43.13c1.71,0.62,3.32,0.59,5.58,0.12c4.83-0.99,24.38-4.62,28.75-5.5c10.94-2.19,3.5,5.25-0.75,10.5"", ""M73.51,48c1.12,1.12,1.57,2.38,1.57,4.41c0,2.04-0.31,35.1-0.31,38.84c0,10.75-7.08,1.18-9.5-0.52""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_091ce"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:091ce"" kvg:element=""野"">
	<g id=""kvg:091ce-g1"" kvg:element=""里"" kvg:position=""left"" kvg:radical=""general"">
		<g id=""kvg:091ce-g2"" kvg:element=""日"">
			<path id=""kvg:091ce-s1"" kvg:type=""㇑"" d=""M12.5,18.54c1.25,0.96,1.45,2.21,1.62,3.18c0.98,5.66,2.66,16.78,3.4,25.76c0.08,1,0.15,1.98,0.21,2.93""/>
			<path id=""kvg:091ce-s2"" kvg:type=""㇕a"" d=""M15.03,20.17c10.69-1.68,25.59-3.93,31.58-4.62c2.5-0.29,3.63,1.82,3.4,3.34c-0.89,5.98-2.33,16.31-3.89,25.13c-0.23,1.31-0.45,2.06-0.65,3.21""/>
			<path id=""kvg:091ce-s3"" kvg:type=""㇐a"" d=""M16.74,34.44c2.97-0.38,29.08-3.9,31.53-3.98""/>
			<path id=""kvg:091ce-s4"" kvg:type=""㇐a"" d=""M17.7,48.43c6.57-0.22,18.7-2.92,27.77-3.37""/>
		</g>
		<path id=""kvg:091ce-s5"" kvg:type=""㇑a"" d=""M30.97,19.87c0.78,1.13,1.06,1.76,1.08,3.13c0.21,12.86,0.06,50.65,0.06,55.01""/>
		<path id=""kvg:091ce-s6"" kvg:type=""㇐"" d=""M15.75,62.75c1.72,0.5,3.79,0.44,4.95,0.25c7.75-1.25,14.9-3.05,23.1-4.25c1.7-0.25,3.83-0.25,5.45,0""/>
		<path id=""kvg:091ce-s7"" kvg:type=""㇀/㇐"" d=""M12.25,82.31c1.38,1.19,3.39,1.61,6.09,0.69c5.53-1.87,21.25-7.06,30.16-10""/>
	</g>
	<g id=""kvg:091ce-g3"" kvg:element=""予"" kvg:position=""right"" kvg:phon=""予"">
		<path id=""kvg:091ce-s8"" kvg:type=""㇇"" d=""M57.51,16.84c2.11,0.66,4.23,0.51,6.25,0.19c7.36-1.16,19.6-3.31,21.94-3.78c2.6-0.52,3.74,1.71,2.19,3.42c-2.66,2.94-12.79,14.29-14.43,16.39""/>
		<path id=""kvg:091ce-s9"" kvg:type=""㇔"" d=""M63.88,28.5c2.86,1.21,10.28,5.85,11.62,9""/>
		<g id=""kvg:091ce-g4"" kvg:element=""了"">
			<path id=""kvg:091ce-s10"" kvg:type=""㇇a"" d=""M54.42,43.13c1.71,0.62,3.32,0.59,5.58,0.12c4.83-0.99,24.38-4.62,28.75-5.5c10.94-2.19,3.5,5.25-0.75,10.5""/>
			<g id=""kvg:091ce-g5"" kvg:element=""亅"">
				<path id=""kvg:091ce-s11"" kvg:type=""㇚"" d=""M73.51,48c1.12,1.12,1.57,2.38,1.57,4.41c0,2.04-0.31,35.1-0.31,38.84c0,10.75-7.08,1.18-9.5-0.52""/>
			</g>
		</g>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_091ce"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 5.50 24.50)"">1</text>
	<text transform=""matrix(1 0 0 1 15.50 16.50)"">2</text>
	<text transform=""matrix(1 0 0 1 20.25 30.13)"">3</text>
	<text transform=""matrix(1 0 0 1 20.25 45.13)"">4</text>
	<text transform=""matrix(1 0 0 1 34.50 25.50)"">5</text>
	<text transform=""matrix(1 0 0 1 8.50 64.50)"">6</text>
	<text transform=""matrix(1 0 0 1 2.50 84.50)"">7</text>
	<text transform=""matrix(1 0 0 1 58.50 14.50)"">8</text>
	<text transform=""matrix(1 0 0 1 56.50 29.50)"">9</text>
	<text transform=""matrix(1 0 0 1 53.50 40.50)"">10</text>
	<text transform=""matrix(1 0 0 1 64.50 55.50)"">11</text>
</g>
</svg>
",
                OrderIndex = 10,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiItem {
                Id = Guid.Parse("35ca0197-448d-5a3f-a88c-fb22be675ebf"),
                LessonId = lesson7Id,
                Level = "N3",
                Character = "半",
                HanViet = "BÁN",
                Meaning = "Một nửa",
                StrokeCount = 5,
                KunReading = "なか.ba",
                OnReading = "ハン",
                Mnemonic = "Dùng một nhát chém (丨) chia rẽ (丷) mọi thứ ra thành hai (二) phần, tức là chia một nửa (半).",
                ComponentMapJson = @"[{""character"": ""丨"", ""component"": ""丨"", ""name"": ""丨"", ""meaning"": ""丨""}, {""character"": ""丷"", ""component"": ""丷"", ""name"": ""丷"", ""meaning"": ""丷""}, {""character"": ""二"", ""component"": ""二"", ""name"": ""二"", ""meaning"": ""二""}]",
                StrokeDataJson = @"[""M26.02,22.58c3.9,2.41,10.07,9.89,11.04,13.63"", ""M82.75,18.75c0.16,1.19-0.39,2.25-1.01,3.2c-2.57,3.96-7.16,8.76-12.62,12.92"", ""M28.3,48.57c1.78,0.38,5.07,0.21,6.83,0.03c10.62-1.09,29.12-2.99,38.5-3.67c2.97-0.22,4.75-0.17,6.24,0.02"", ""M13.5,67.22c2.25,0.91,6.85,1.03,9.22,0.78c18.28-1.87,40.4-3.58,62.61-4.38c4-0.14,6.41,0.25,8.42,0.52"", ""M52.67,11.5c1.23,1.23,1.85,3.17,1.85,4.4c0,7.6,0.1,54.35,0.1,75.1c0,3.18-0.07,5.46-0.11,6.5""]",
                StrokeSvg = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!--
Copyright (C) 2009/2010/2011 Ulrich Apel.
This work is distributed under the conditions of the Creative Commons
Attribution-Share Alike 3.0 Licence. This means you are free:
* to Share - to copy, distribute and transmit the work
* to Remix - to adapt the work

Under the following conditions:
* Attribution. You must attribute the work by stating your use of KanjiVG in
  your own copyright header and linking to KanjiVG's website
  (http://kanjivg.tagaini.net)
* Share Alike. If you alter, transform, or build upon this work, you may
  distribute the resulting work only under the same or similar license to this
  one.

See http://creativecommons.org/licenses/by-sa/3.0/ for more details.
-->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.0//EN"" ""http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd"" [
<!ATTLIST g
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:element CDATA #IMPLIED
kvg:variant CDATA #IMPLIED
kvg:partial CDATA #IMPLIED
kvg:original CDATA #IMPLIED
kvg:part CDATA #IMPLIED
kvg:number CDATA #IMPLIED
kvg:tradForm CDATA #IMPLIED
kvg:radicalForm CDATA #IMPLIED
kvg:position CDATA #IMPLIED
kvg:radical CDATA #IMPLIED
kvg:phon CDATA #IMPLIED >
<!ATTLIST path
xmlns:kvg CDATA #FIXED ""http://kanjivg.tagaini.net""
kvg:type CDATA #IMPLIED >
]>
<svg xmlns=""http://www.w3.org/2000/svg"" width=""109"" height=""109"" viewBox=""0 0 109 109"">
<g id=""kvg:StrokePaths_0534a"" style=""fill:none;stroke:#000000;stroke-width:3;stroke-linecap:round;stroke-linejoin:round;"">
<g id=""kvg:0534a"" kvg:element=""半"">
	<g id=""kvg:0534a-g1"" kvg:element=""丶"" kvg:radical=""nelson"">
		<path id=""kvg:0534a-s1"" kvg:type=""㇔"" d=""M26.02,22.58c3.9,2.41,10.07,9.89,11.04,13.63""/>
	</g>
	<path id=""kvg:0534a-s2"" kvg:type=""㇒"" d=""M82.75,18.75c0.16,1.19-0.39,2.25-1.01,3.2c-2.57,3.96-7.16,8.76-12.62,12.92""/>
	<g id=""kvg:0534a-g2"" kvg:element=""二"" kvg:part=""1"">
		<path id=""kvg:0534a-s3"" kvg:type=""㇐"" d=""M28.3,48.57c1.78,0.38,5.07,0.21,6.83,0.03c10.62-1.09,29.12-2.99,38.5-3.67c2.97-0.22,4.75-0.17,6.24,0.02""/>
	</g>
	<g id=""kvg:0534a-g3"" kvg:element=""十"" kvg:radical=""tradit"">
		<g id=""kvg:0534a-g4"" kvg:element=""二"" kvg:part=""2"">
			<path id=""kvg:0534a-s4"" kvg:type=""㇐"" d=""M13.5,67.22c2.25,0.91,6.85,1.03,9.22,0.78c18.28-1.87,40.4-3.58,62.61-4.38c4-0.14,6.41,0.25,8.42,0.52""/>
		</g>
		<path id=""kvg:0534a-s5"" kvg:type=""㇑"" d=""M52.67,11.5c1.23,1.23,1.85,3.17,1.85,4.4c0,7.6,0.1,54.35,0.1,75.1c0,3.18-0.07,5.46-0.11,6.5""/>
	</g>
</g>
</g>
<g id=""kvg:StrokeNumbers_0534a"" style=""font-size:8;fill:#808080"">
	<text transform=""matrix(1 0 0 1 19.50 23.50)"">1</text>
	<text transform=""matrix(1 0 0 1 73.50 18.50)"">2</text>
	<text transform=""matrix(1 0 0 1 21.50 49.50)"">3</text>
	<text transform=""matrix(1 0 0 1 5.50 68.50)"">4</text>
	<text transform=""matrix(1 0 0 1 44.50 12.50)"">5</text>
</g>
</svg>
",
                OrderIndex = 11,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
        };

        foreach (var item in kanjiItems7)
        {
            if (!await db.KanjiItems.AnyAsync(k => k.Character == item.Character))
                db.KanjiItems.Add(item);
        }

        var vocabItems7 = new List<KanjiVocabulary>
        {
            new KanjiVocabulary {
                Id = Guid.Parse("97873265-06bf-5583-9426-c0d9429ebc06"),
                LessonId = lesson7Id,
                KanjiItemId = Guid.Parse("465ad202-c37c-571a-985c-69c96c90a421"),
                Level = "N3",
                Word = "肉",
                Reading = "にく",
                Meaning = "thịt",
                OrderIndex = 1,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("06084590-1b3f-5674-bb73-59d6dd8587e2"),
                LessonId = lesson7Id,
                KanjiItemId = Guid.Parse("465ad202-c37c-571a-985c-69c96c90a421"),
                Level = "N3",
                Word = "牛肉",
                Reading = "ぎゅうにく",
                Meaning = "thịt bò",
                OrderIndex = 2,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("9c11ed8c-8145-5f2a-a711-8d3d5af33b4f"),
                LessonId = lesson7Id,
                KanjiItemId = Guid.Parse("465ad202-c37c-571a-985c-69c96c90a421"),
                Level = "N3",
                Word = "豚肉",
                Reading = "ぶたにく",
                Meaning = "thịt heo",
                OrderIndex = 3,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("1e006f0e-10b0-5e99-a942-e434cedf6f3c"),
                LessonId = lesson7Id,
                KanjiItemId = Guid.Parse("465ad202-c37c-571a-985c-69c96c90a421"),
                Level = "N3",
                Word = "鶏肉",
                Reading = "とりにく",
                Meaning = "thịt gà",
                OrderIndex = 4,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("74de8ff4-4705-5c63-bddb-98a187bc291f"),
                LessonId = lesson7Id,
                KanjiItemId = Guid.Parse("8dc8753c-940a-55fa-8156-1436615d7e72"),
                Level = "N3",
                Word = "料金",
                Reading = "りょうきん",
                Meaning = "tiền phí",
                OrderIndex = 5,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("9c0cf39c-f85e-546b-bd1b-b24ef40006b5"),
                LessonId = lesson7Id,
                KanjiItemId = Guid.Parse("8dc8753c-940a-55fa-8156-1436615d7e72"),
                Level = "N3",
                Word = "料理",
                Reading = "りょうり",
                Meaning = "món ăn",
                OrderIndex = 6,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("57f5cc33-682c-5950-b787-8d59818d8b40"),
                LessonId = lesson7Id,
                KanjiItemId = Guid.Parse("01b783d3-c933-5a04-aba8-e7af559397e1"),
                Level = "N3",
                Word = "野菜",
                Reading = "やさい",
                Meaning = "rau",
                OrderIndex = 7,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("ec302d99-9384-58e1-8636-d145692d35ea"),
                LessonId = lesson7Id,
                KanjiItemId = Guid.Parse("35ca0197-448d-5a3f-a88c-fb22be675ebf"),
                Level = "N3",
                Word = "半年",
                Reading = "はんとし/はんねん",
                Meaning = "nửa năm",
                OrderIndex = 8,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("67c9f26b-8a93-5ed2-a174-6a49de5641ab"),
                LessonId = lesson7Id,
                KanjiItemId = Guid.Parse("35ca0197-448d-5a3f-a88c-fb22be675ebf"),
                Level = "N3",
                Word = "半分",
                Reading = "はんぶん",
                Meaning = "Một nửa",
                OrderIndex = 9,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("4bdb4f9e-35f6-570d-afc4-088995ab0652"),
                LessonId = lesson7Id,
                KanjiItemId = Guid.Parse("35ca0197-448d-5a3f-a88c-fb22be675ebf"),
                Level = "N3",
                Word = "半日",
                Reading = "はんじつ/はんにち",
                Meaning = "Nửa ngày",
                OrderIndex = 10,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("9eeb45cc-07d3-5851-a691-3c67a37e34f1"),
                LessonId = lesson7Id,
                KanjiItemId = Guid.Parse("35ca0197-448d-5a3f-a88c-fb22be675ebf"),
                Level = "N3",
                Word = "半月",
                Reading = "はんつき",
                Meaning = "Nửa tháng",
                OrderIndex = 11,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("cbc39499-59a6-5219-8561-fe5eaaa2b137"),
                LessonId = lesson7Id,
                KanjiItemId = Guid.Parse("35ca0197-448d-5a3f-a88c-fb22be675ebf"),
                Level = "N3",
                Word = "七時半",
                Reading = "しちじはん",
                Meaning = "7h30",
                OrderIndex = 12,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("7dc6cd31-9925-5804-8435-b592928b5fe2"),
                LessonId = lesson7Id,
                KanjiItemId = null,
                Level = "N3",
                Word = "大きい",
                Reading = "おおきい",
                Meaning = "to",
                OrderIndex = 13,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("5e359021-838e-57ad-a98b-533873a26101"),
                LessonId = lesson7Id,
                KanjiItemId = null,
                Level = "N3",
                Word = "大学生",
                Reading = "だいがくせい",
                Meaning = "Sinh viên đại học",
                OrderIndex = 14,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("746a3f10-add0-5e6d-ad1a-d4d277f59f38"),
                LessonId = lesson7Id,
                KanjiItemId = null,
                Level = "N3",
                Word = "大人",
                Reading = "おとな",
                Meaning = "Người lớn",
                OrderIndex = 15,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("c538846d-ae24-5e92-8d94-58ee8a6bd64d"),
                LessonId = lesson7Id,
                KanjiItemId = null,
                Level = "N3",
                Word = "大会",
                Reading = "たいかい",
                Meaning = "đại hội",
                OrderIndex = 16,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("240d1d5d-b9a0-5ba0-8b6e-1a167a6b8a71"),
                LessonId = lesson7Id,
                KanjiItemId = Guid.Parse("35ca0197-448d-5a3f-a88c-fb22be675ebf"),
                Level = "N3",
                Word = "大半",
                Reading = "たいはん",
                Meaning = "Phần lớn",
                OrderIndex = 17,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("6278ad70-193e-51c6-94d1-d75f75d66f42"),
                LessonId = lesson7Id,
                KanjiItemId = null,
                Level = "N3",
                Word = "大学",
                Reading = "だいがく",
                Meaning = "đại học",
                OrderIndex = 18,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("eff532b6-9fc7-5e3a-aa37-50a4d2ab958f"),
                LessonId = lesson7Id,
                KanjiItemId = null,
                Level = "N3",
                Word = "小さい",
                Reading = "ちいさい",
                Meaning = "bé, nhỏ",
                OrderIndex = 19,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("00655620-1e0e-5b64-8f5b-aca0a4c81e05"),
                LessonId = lesson7Id,
                KanjiItemId = null,
                Level = "N3",
                Word = "小学校",
                Reading = "しょうがっこう",
                Meaning = "Trường tiểu học",
                OrderIndex = 20,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("dd91fb29-edd7-512a-8658-c64426968315"),
                LessonId = lesson7Id,
                KanjiItemId = null,
                Level = "N3",
                Word = "小学生",
                Reading = "しょうがくせい",
                Meaning = "học sinh tiểu học",
                OrderIndex = 21,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
            new KanjiVocabulary {
                Id = Guid.Parse("594a36d4-9780-598b-aadc-642b55a4a661"),
                LessonId = lesson7Id,
                KanjiItemId = null,
                Level = "N3",
                Word = "小人",
                Reading = "こびと",
                Meaning = "đứa trẻ, người tí hon",
                OrderIndex = 22,
                CreatedAt = SeededAt,
                UpdatedAt = SeededAt
            },
        };

        foreach (var item in vocabItems7)
        {
            if (!await db.KanjiVocabularyItems.AnyAsync(v => v.Word == item.Word && v.Reading == item.Reading))
                db.KanjiVocabularyItems.Add(item);
        }

    }
}
