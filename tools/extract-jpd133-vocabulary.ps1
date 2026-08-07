Add-Type -AssemblyName System.IO.Compression.FileSystem

$source = 'docs/JPD133/Từ vựng L1-L15.docx'
$target = 'docs/JPD133/Resource/JPD133_Tu_vung_Bai_8_11_chi_tiet.md'
$zip = [IO.Compression.ZipFile]::OpenRead($source)
$entry = $zip.GetEntry('word/document.xml')
$reader = [IO.StreamReader]::new($entry.Open())
$xml = [xml]$reader.ReadToEnd()
$reader.Dispose()
$zip.Dispose()

$ns = [Xml.XmlNamespaceManager]::new($xml.NameTable)
$ns.AddNamespace('w', 'http://schemas.openxmlformats.org/wordprocessingml/2006/main')
$paragraphs = $xml.SelectNodes('//w:body//w:p', $ns)

$groups = @(
  @{ Key = '8_1'; Title = '家族や友達'; Vietnamese = 'Gia đình và bạn bè'; Start = 577; End = 616 },
  @{ Key = '8_2'; Title = 'こんな人'; Vietnamese = 'Những người như thế này'; Start = 618; End = 647 },
  @{ Key = '8_3'; Title = 'プレゼント'; Vietnamese = 'Quà tặng'; Start = 649; End = 675 },
  @{ Key = '9_1'; Title = 'いろいろな趣味'; Vietnamese = 'Nhiều sở thích'; Start = 680; End = 747 },
  @{ Key = '9_2'; Title = 'できること・できないこと'; Vietnamese = 'Việc có thể và không thể làm'; Start = 749; End = 768 },
  @{ Key = '9_3'; Title = '楽しい週末'; Vietnamese = 'Cuối tuần vui vẻ'; Start = 770; End = 786 },
  @{ Key = '10_1'; Title = '私の集合'; Vietnamese = 'Tập trung'; Start = 790; End = 818 },
  @{ Key = '10_2'; Title = 'いろいろな注意'; Vietnamese = 'Các lưu ý'; Start = 820; End = 842 },
  @{ Key = '10_3'; Title = '動物園で'; Vietnamese = 'Ở vườn thú'; Start = 844; End = 875 },
  @{ Key = '11_1'; Title = '今の生活'; Vietnamese = 'Cuộc sống hiện tại'; Start = 879; End = 906 },
  @{ Key = '11_2'; Title = '今の私・前の私'; Vietnamese = 'Tôi hiện tại và tôi trước đây'; Start = 908; End = 922 },
  @{ Key = '11_3'; Title = '友達と'; Vietnamese = 'Cùng bạn bè'; Start = 924; End = 935 }
)

function Get-Columns($paragraph) {
  $columns = @('')
  foreach ($child in $paragraph.ChildNodes) {
    if ($child.LocalName -ne 'r') { continue }
    foreach ($runChild in $child.ChildNodes) {
      if ($runChild.LocalName -eq 't') { $columns[-1] += $runChild.InnerText }
      elseif ($runChild.LocalName -eq 'tab') { $columns += @('') }
    }
  }
  return @($columns | ForEach-Object { $_.Trim() })
}

function Is-Japanese($text) { return $text -match '[ぁ-ゖァ-ヺ一-龯々ー]' }
function Is-Reading($text) { return $text -match '^[ぁ-ゖァ-ヺー（）・［］\[\]\s]+$' }
function Clean-Word($word) {
  $word = $word.Trim() -replace '^\d+\s*', ''
  $word = $word -replace '(［[^］]+］|「[^」]+」)\s*[123]?\s*.*$', '$1'
  $word = $word -replace '\s+[123]$', ''
  return $word.Trim()
}
function Get-Reading($word, $columns) {
  $candidate = @($columns | Where-Object { $_ -and (Is-Reading $_) -and $_ -ne $word } | Select-Object -First 1)
  if ($candidate.Count -gt 0) { return $candidate[0] }
  $bracket = [regex]::Match($word, '［[^］]+］')
  if ($bracket.Success) { return $word.Substring($bracket.Index + 1, $bracket.Length - 2) }
  if ($word -match '^[ぁ-ゖァ-ヺー（）・]+$') { return $word }
  return '—'
}
function Get-WordType($word, $meaning) {
  if ($word -match '［|「') { return 'Động từ' }
  if ($word -match '（な）') { return 'Tính từ な' }
  if ($word -match '～|一人|二人|何人|一匹|六匹|八匹|何匹|一回|六回|八回|十回|何回|一冊|十冊|何冊|一杯|六杯|八杯|十杯|何杯|一本|六本|八本|十本|何本') { return 'Từ/cụm đếm' }
  $adverbs = 'いつも|よく|ときどき|あまり|全然|でも|だけ|特に|最近|だんだん|初めて|それで|たいてい|なかなか|ええと|まっすぐ|ちょっと|そろそろ|本当だ|どうやって|もうすぐ|よかったですね|うん|ううん|ごめん|そっか|また'
  if ($word -match "^(?:$adverbs)$") { return 'Phó từ/cụm cố định' }
  if ($word -match 'い$|しい$|たい$|ない$') { return 'Tính từ い' }
  return 'Danh từ/cụm từ'
}
function To-Dictionary-Reading($reading) {
  $reading = $reading.Trim()
  if ($reading -match 'います$') { return $reading -replace 'います$', 'う' }
  if ($reading -match 'きます$') { return $reading -replace 'きます$', 'く' }
  if ($reading -match 'ぎます$') { return $reading -replace 'ぎます$', 'ぐ' }
  if ($reading -match 'します$') { return $reading -replace 'します$', 'する' }
  if ($reading -match 'ちます$') { return $reading -replace 'ちます$', 'つ' }
  if ($reading -match 'にます$') { return $reading -replace 'にます$', 'ぬ' }
  if ($reading -match 'びます$') { return $reading -replace 'びます$', 'ぶ' }
  if ($reading -match 'みます$') { return $reading -replace 'みます$', 'む' }
  if ($reading -match 'ります$') { return $reading -replace 'ります$', 'る' }
  if ($reading -match 'えます$') { return $reading -replace 'えます$', 'える' }
  return $reading
}
function Make-Example($key, $word, $reading, $meaning) {
  $base = ($word -replace '［[^］]+］|「[^」]+」', '').Trim()
  $base = $base -replace '（な）', ''
  $dictionary = ([regex]::Match($word, '［([^］]+)］|「([^」]+)」')).Groups | Where-Object { $_.Value -and $_.Name -in @('1','2') } | Select-Object -First 1 -ExpandProperty Value
  if (-not $dictionary) { $dictionary = $base }
  $dictionaryReading = To-Dictionary-Reading $reading
  $te = @{ '押します' = '押して'; '座ります' = '座って'; '立ちます' = '立って'; '入ります' = '入って'; '持って帰ります' = '持って帰って'; '遅れます' = '遅れて'; '捨てます' = '捨てて' }[$base]
  if (-not $te) { $te = $base }
  $teReading = @{ 'おします' = 'おして'; 'すわります' = 'すわって'; 'たちます' = 'たって'; 'はいります' = 'はいって'; 'もってかえります' = 'もってかえって'; 'おくれます' = 'おくれて'; 'すてます' = 'すてて' }[$reading]
  if (-not $teReading) { $teReading = $reading }
  switch -Regex ($key) {
    '^8_1$' { if ($base -match '住み') { return @('私は東京に住んでいます。','わたしはとうきょうにすんでいます。','Tôi đang sống ở Tokyo.','Dùng Vて形＋います để diễn tả trạng thái cư trú.') }; if ($base -match 'います') { return @('弟がいます。','おとうとがいます。','Tôi có em trai.','Dùng います cho người và động vật.') }; if ($base -match '～') { return @("私は${base}で住んでいます。", "わたしは${reading}ですんでいます。", "Tôi sống cùng số lượng được nêu.",'Dùng ～人で để nói tổng số người cùng sống.') }; return @("私は${base}と住んでいます。", "わたしは${reading}とすんでいます。", "Tôi sống cùng ${meaning}.", 'Dùng と để chỉ người cùng sống trong Bài 8.') }
    '^8_2$' { if ($base -match '体|足|顔|髪|口|鼻|目|耳') { return @("母は${base}が長いです。", "ははは${reading}がながいです。", "Mẹ tôi có ${meaning} dài.", 'Dùng mẫu N1はN2がAです để miêu tả đặc điểm.') }; if ($word -match '（な）') { return @("母は${base}です。", "ははは${reading -replace '（な）',''}です。", "Mẹ tôi ${meaning}.", 'Dùng tính từ な trong câu miêu tả.') }; return @("${base}さんは親切です。", "${reading}さんはしんせつです。", "${meaning} thì tốt bụng.", 'Dùng tính từ để miêu tả người trong Bài 8.') }
    '^8_3$' { if ($base -match 'もら') { return @('私は母にプレゼントをもらいました。','わたしはははにプレゼントをもらいました。','Tôi đã nhận quà từ mẹ.','Dùng に cho người cho trong mẫu もらいます.') }; if ($base -match 'くれ') { return @('友達が私にカードをくれました。','ともだちがわたしにカードをくれました。','Bạn đã tặng tôi một tấm thiệp.','Dùng くれます khi quà hướng về phía người nói.') }; if ($base -match '電話') { return @('友達に電話します。','ともだちにでんわします。','Tôi gọi điện cho bạn.','Dùng に để chỉ đối tượng liên lạc.') }; if ($base -match '送') { return @('友達に手紙を送ります。','ともだちにてがみをおくります。','Tôi gửi thư cho bạn.','Dùng に chỉ người nhận.') }; if ($base -match 'あげ') { return @('私は友達にカードをあげました。','わたしはともだちにカードをあげました。','Tôi đã tặng bạn một tấm thiệp.','Dùng mẫu N1にN2をあげます.') }; return @("私は友達に${base}をあげました。", "わたしはともだちに${reading}をあげました。", "Tôi đã tặng bạn một món quà.", 'Dùng mẫu N1にN2をあげます.') }
    '^9_1$' { if ($base -match '描き') { return @('私は絵を描きます。','わたしはえをかきます。','Tôi vẽ tranh.','Dùng động từ thể ます trong câu nói về sở thích.') }; if ($base -match '集め') { return @('私は切手を集めます。','わたしはきってをあつめます。','Tôi sưu tầm tem.','Dùng động từ thể ます.') }; if ($base -match '運転') { return @('私は車を運転します。','わたしはくるまをうんてんします。','Tôi lái xe.','Dùng động từ thể ます.') }; if ($base -match '泳') { return @('私はプールで泳ぎます。','わたしはプールでおよぎます。','Tôi bơi ở bể bơi.','Dùng で chỉ nơi diễn ra hoạt động.') }; if ($base -match '～日|一日|何日') { return @('一週間に五日、日本語を勉強します。','いっしゅうかんにいつか、にほんごをべんきょうします。','Một tuần tôi học tiếng Nhật năm ngày.','Dùng lượng từ chỉ thời gian trong Bài 9.') }; if ($base -match '～週間') { return @('二週間に一回、映画を見ます。','にしゅうかんにいっかい、えいがをみます。','Cứ hai tuần tôi xem phim một lần.','Dùng に để chỉ chu kỳ tần suất.') }; if ($base -match 'か月|～年') { return @('一か月に三冊、本を読みます。','いっかげつにさんさつ、ほんをよみます。','Mỗi tháng tôi đọc ba quyển sách.','Dùng mẫu khoảng thời gian に số lượng.') }; if ($base -match '回|冊|杯|本') { return @('一週間に二回、家族に電話します。','いっしゅうかんににかい、かぞくにでんわします。','Một tuần tôi gọi cho gia đình hai lần.','Dùng lượng từ chỉ số lần/số lượng.') }; if ($base -match '全然') { return @('最近、全然スポーツをしません。','さいきん、ぜんぜんスポーツをしません。','Gần đây tôi hoàn toàn không chơi thể thao.','全然 thường đi với phủ định trong phạm vi bài.') }; if ($base -match '特に') { return @('私は特に音楽が好きです。','わたしはとくにおんがくがすきです。','Tôi đặc biệt thích âm nhạc.','Dùng phó từ 特に.') }; return @("私は${base}が好きです。", "わたしは${reading}がすきです。", "Tôi thích ${meaning}.",'Dùng Nが好きです trong Bài 9.') }
    '^9_2$' { if ($word -match '［') { return @("私は${dictionary}ことができます。", "わたしは${dictionaryReading}ことができます。", "Tôi có thể làm việc này.",'Dùng V辞書形＋ことができます để nói khả năng.') }; return @("私は${base}ができます。", "わたしは${reading}ができます。", "Tôi có thể ${meaning}.",'Dùng Nができます để nói khả năng.') }
    '^9_3$' { if ($base -match '受付') { return @('図書館の受付でカードを作ります。','としょかんのうけつけでカードをつくります。','Tôi làm thẻ ở quầy lễ tân thư viện.','Dùng で chỉ nơi diễn ra hành động.') }; if ($base -match '外国人登録証') { return @('外国人登録証を見せます。','がいこくじんとうろくしょうをみせます。','Tôi cho xem giấy đăng ký người nước ngoài.','Dùng を với vật được đưa ra/cho xem.') }; if ($base -match '住所') { return @('住所を言います。','じゅうしょをいいます。','Tôi nói địa chỉ.','Dùng を với nội dung được nói.') }; if ($base -match '宿題') { return @('週末、宿題をします。','しゅうまつ、しゅくだいをします。','Cuối tuần tôi làm bài tập.') }; if ($base -match '電話番号') { return @('電話番号を言います。','でんわばんごうをいいます。','Tôi nói số điện thoại.') }; if ($base -match '～番') { return @('3番のバスに乗ります。','さんばんのバスにのります。','Tôi lên xe buýt số 3.','Dùng số thứ tự trong chỉ dẫn.') }; if ($base -match '言います') { return @('住所を言います。','じゅうしょをいいます。','Tôi nói địa chỉ.','Dùng động từ 言います.') }; if ($base -match '払います') { return @('お金を払います。','おかねをはらいます。','Tôi trả tiền.','Dùng を với vật được thanh toán.') }; if ($base -match '降ります') { return @('バスを降ります。','バスをおります。','Tôi xuống xe buýt.','Dùng を với phương tiện rời khỏi.') }; if ($base -match '見せます') { return @('カードを見せます。','カードをみせます。','Tôi cho xem thẻ.') }; if ($base -match '予約します') { return @('レストランを予約します。','レストランをよやくします。','Tôi đặt chỗ nhà hàng.') }; return @("週末、友達と${base}。", "しゅうまつ、ともだちと${reading}。", "Cuối tuần tôi làm hoạt động này với bạn.",'Dùng câu hỏi/câu trả lời về cách thực hiện trong Bài 9.') }
    '^10_1$' { if ($base -match '音|声') { return @('鳥の声が聞こえます。','とりのこえがきこえます。','Có thể nghe thấy tiếng chim.','Dùng Nが聞こえます.') }; if ($base -match '見え') { return @('ここから橋が見えます。','ここからはしがみえます。','Từ đây có thể nhìn thấy cây cầu.','Dùng Nが見えます.') }; if ($base -match '飲み') { return @('薬を飲みます。','くすりをのみます。','Tôi uống thuốc.','Dùng を cho vật được tác động.') }; if ($base -match '右|左') { return @("交差点を${base}に曲がります。", "こうさてんを${reading}にまがります。", "Tôi rẽ ${meaning} ở ngã tư.",'Dùng を với địa điểm đi qua và に với hướng.') }; if ($base -match '橋') { return @('あの橋を渡ります。','あのはしをわたります。','Tôi qua cây cầu kia.','Dùng を với địa điểm/tuyến đường.') }; if ($base -match '道') { return @('この道をまっすぐ行きます。','このみちをまっすぐいきます。','Tôi đi thẳng theo con đường này.','Dùng を với tuyến đường.') }; if ($base -match '交差点|角|信号') { return @('交差点を右に曲がります。','こうさてんをみぎにまがります。','Tôi rẽ phải ở ngã tư.','Dùng を trong chỉ đường.') }; return @("この道を${base}。", "このみちを${reading}。", "Tôi ${meaning} theo con đường này.",'Dùng を với địa điểm/tuyến đường trong Bài 10.') }
    '^10_2$' { if ($base -match '客|皆さん') { return @('お客さんがいます。','おきゃくさんがいます。','Có khách ở đây.','Dùng います cho người.') }; if ($base -match '手') { return @('手を洗います。','てをあらいます。','Tôi rửa tay.','Dùng を với bộ phận chịu tác động.') }; if ($word -match '［') { return @("ここで${te}ください。", "ここで${teReading}ください。", "Xin hãy thực hiện hành động này ở đây.",'Dùng Vてください để đưa ra yêu cầu.') }; if ($word -match '（な）') { return @("これは${base}です。", "これは${reading -replace '（な）',''}です。", "Cái này thì ${meaning}.",'Dùng tính từ な trong Bài 10.') }; return @("ここに${base}があります。", "ここに${reading}があります。", "Ở đây có ${meaning}.",'Dùng あります để nói sự tồn tại của đồ vật.') }
    '^10_3$' { if ($word -match '［|「') { return @("サルが${base}。", "サルが${reading}。", "Con khỉ đang thực hiện hành động này.",'Dùng NがVています để mô tả tình huống đang diễn ra.') }; if ($base -match 'クマ|コアラ|サル|ゾウ|鳥|パンダ|ペンギン') { return @("動物園に${base}がいます。", "どうぶつえんに${reading}がいます。", "Ở vườn thú có ${meaning}.",'Dùng います với động vật trong Bài 10.') }; return @("動物園に${base}があります。", "どうぶつえんに${reading}があります。", "Ở vườn thú có ${meaning}.",'Dùng あります với địa điểm/đồ vật trong Bài 10.') }
    '^11_1$' { if ($word -match '［') { return @("毎日${base}。", "まいにち${reading}。", "Mỗi ngày tôi thực hiện hoạt động này.",'Dùng Vています để diễn tả thói quen khi phù hợp.') }; return @("毎日、${base}について話します。", "まいにち、${reading}についてはなします。", "Mỗi ngày tôi nói về ${meaning}.",'Dùng について và thói quen trong Bài 11.') }
    '^11_2$' { if ($word -match '［') { return @("子どものとき、${base}。", "こどものとき、${reading}。", "Khi còn nhỏ, tôi đã thực hiện hoạt động này.",'Dùng Vた形＋とき trong Bài 11.') }; return @("子どものとき、${base}でした。", "こどものとき、${reading}でした。", "Khi còn nhỏ, tôi là ${meaning}.",'Dùng Nのとき trong Bài 11.') }
    '^11_3$' { if ($word -match '［') { return @("明日、${dictionary}？", "あした、${dictionaryReading}？", "Ngày mai làm việc này không?",'Dùng câu hỏi thể thường thân mật.') }; return @("明日、${base}？", "あした、${reading}？", "Ngày mai ${meaning} không?",'Dùng câu hỏi thể thường thân mật.') }
  }
}

$out = @('# JPD133 – Từ vựng Bài 8–11', '', '> Nguồn chính: `Từ vựng L1-L15.docx`; đối chiếu cấu trúc với `01-tu-vung.md` và ngữ pháp JPD133 Bài 8–11.', '')
$counts = @{}
foreach ($group in $groups) {
  $items = @()
  for ($i = $group.Start; $i -lt $group.End; $i++) {
    $columns = Get-Columns $paragraphs[$i]
    $raw = ($columns -join '')
    if (-not $raw.Trim() -or $raw -match '。' -or $raw -match '^第\d+課|ことば|家族・友達|プレゼント|趣味|集合|注意|動物園|生活|前の私|友達と') { continue }
    $nonEmpty = @($columns | Where-Object { $_ -and $_.Trim() })
    if ($nonEmpty.Count -lt 2 -or -not (Is-Japanese $raw)) { continue }
    $word = Clean-Word $nonEmpty[0]
    if (-not $word -or $word -match '^[ぁ-ゖァ-ヺー]+。?$' -and $nonEmpty.Count -lt 3) { continue }
    $meaning = $nonEmpty[-1]
    if ($meaning -eq $word -or $meaning -match '^(例：|Vd:)') { continue }
    $reading = Get-Reading $word $columns
    $example = Make-Example $group.Key $word $reading $meaning
    $items += ,@($word, $reading, (Get-WordType $word $meaning), $meaning, $example[0], $example[1], $example[2], $example[3])
  }
  $counts[$group.Key] = $items.Count
  $out += "## $($group.Key) – $($group.Title) / $($group.Vietnamese)", '', '| STT | Từ vựng | Cách đọc | Loại từ | Nghĩa | Ví dụ tiếng Nhật | Cách đọc ví dụ | Dịch nghĩa | Ghi chú |', '|---:|---|---|---|---|---|---|---|---|'
  $index = 1
  foreach ($item in $items) {
    $cells = @($index) + $item
    $out += '| ' + (($cells | ForEach-Object { ("$_" -replace '[\r\n]+', ' ') -replace '\|','/' }) -join ' | ') + ' |'
    $index++
  }
  $out += '', "Tổng số mục: **$($items.Count)**.", ''
}
$out += '## Đối chiếu', '', '- Các nhóm được giữ theo đúng thứ tự xuất hiện trong DOCX.', '- Các dòng câu mẫu trong DOCX được dùng để tham chiếu, không tạo thành mục từ vựng riêng.', '- Dấu `—` ở cách đọc chỉ xuất hiện khi DOCX không ghi rõ cách đọc trong phần XML; không tự đoán dữ liệu nguồn.', ''
[IO.File]::WriteAllText($target, ($out -join "`n"), [Text.UTF8Encoding]::new($false))
$counts.GetEnumerator() | Sort-Object Name | ForEach-Object { "$($_.Name)=$($_.Value)" }
