import { mkdir, writeFile } from 'node:fs/promises';
import path from 'node:path';

const root = process.cwd();
const lessons = [
  {
    id: 8, topic: 'Gia đình', title: '家族と友達', subtitle: '25 đoạn đọc về gia đình, nơi sống và quà tặng.', summary: 'Bài đọc luyện Vています, số người, miêu tả người và cho nhận.', titles: ['Gia đình tôi', 'Nhà mới', 'Sống cùng bố mẹ', 'Anh trai', 'Chị gái', 'Em gái', 'Con chó của tôi', 'Bạn cùng phòng', 'Bố tôi', 'Mẹ tôi', 'Ông bà', 'Gia đình cuối tuần', 'Bữa tối gia đình', 'Sinh nhật', 'Quà sinh nhật', 'Bạn thân', 'Gia đình ở quê', 'Gia đình ở thành phố', 'Một ngày nghỉ', 'Đi thăm ông bà', 'Gia đình hạnh phúc', 'Mẹ nấu ăn', 'Con mèo', 'Người hàng xóm', 'Người quan trọng nhất'],
    texts: [
      ['私は家族と四人で住んでいます。', 'Tôi sống cùng gia đình, tổng cộng bốn người.', 'watashi wa kazoku to yonin de sundeimasu.'], ['新しい家は学校の近くにあります。', 'Ngôi nhà mới ở gần trường.', 'atarashii ie wa gakkou no chikaku ni arimasu.'], ['私は両親と住んでいます。', 'Tôi sống cùng bố mẹ.', 'watashi wa ryoushin to sundeimasu.'], ['兄は東京に住んでいます。', 'Anh trai tôi sống ở Tokyo.', 'ani wa toukyou ni sundeimasu.'], ['姉は病院で働いています。', 'Chị gái tôi làm việc ở bệnh viện.', 'ane wa byouin de hataraiteimasu.'], ['妹は大学生で、親切です。', 'Em gái tôi là sinh viên đại học và tốt bụng.', 'imouto wa daigakusei de shinsetsu desu.'], ['犬が一匹います。', 'Tôi có một con chó.', 'inu ga ippiki imasu.'], ['ルームメイトと三人で住んでいます。', 'Tôi sống cùng bạn cùng phòng, tổng cộng ba người.', 'ruumumeito to sannin de sundeimasu.'], ['父は会社で働いています。', 'Bố tôi làm việc ở công ty.', 'chichi wa kaisha de hataraiteimasu.'], ['母は料理を作っています。', 'Mẹ tôi đang nấu ăn.', 'haha wa ryouri o tsukutteimasu.'], ['祖父母は田舎に住んでいます。', 'Ông bà tôi sống ở quê.', 'sofubo wa inaka ni sundeimasu.'], ['週末、家族と公園へ行きます。', 'Cuối tuần tôi đi công viên với gia đình.', 'shuumatsu, kazoku to kouen e ikimasu.'], ['晩ご飯は家族と一緒に食べます。', 'Tôi ăn tối cùng gia đình.', 'bangohan wa kazoku to issho ni tabemasu.'], ['誕生日に友達がカードをくれました。', 'Sinh nhật, bạn đã tặng tôi một tấm thiệp.', 'tanjoubi ni tomodachi ga kaado o kuremashita.'], ['私は母に花をあげました。', 'Tôi đã tặng hoa cho mẹ.', 'watashi wa haha ni hana o agemashita.'], ['親友は明るくて、やさしい人です。', 'Bạn thân của tôi vui vẻ và hiền.', 'shinyuu wa akarukute, yasashii hito desu.'], ['田舎の家族は元気です。', 'Gia đình ở quê khỏe mạnh.', 'inaka no kazoku wa genki desu.'], ['町の家は便利で、静かです。', 'Ngôi nhà trong thành phố tiện lợi và yên tĩnh.', 'machi no ie wa benri de, shizuka desu.'], ['休みの日、家で本を読みます。', 'Ngày nghỉ tôi đọc sách ở nhà.', 'yasumi no hi, ie de hon o yomimasu.'], ['日曜日、祖父母の家へ行きます。', 'Chủ nhật tôi đi đến nhà ông bà.', 'nichiyoubi, sofubo no ie e ikimasu.'], ['私の家族は仲がいいです。', 'Gia đình tôi hòa thuận.', 'watashi no kazoku wa naka ga ii desu.'], ['母は台所で料理を作っています。', 'Mẹ đang nấu ăn trong bếp.', 'haha wa daidokoro de ryouri o tsukutteimasu.'], ['猫が二匹います。', 'Tôi có hai con mèo.', 'neko ga nihiki imasu.'], ['隣の人は親切です。', 'Người hàng xóm tốt bụng.', 'tonari no hito wa shinsetsu desu.'], ['家族は私にとって大切な人です。', 'Gia đình là những người quan trọng đối với tôi.', 'kazoku wa watashi ni totte taisetsu na hito desu.'],
    ], grammar: 'JPD133-L08-G001 đến G012', vocab: 'Gia đình, người thân, nơi sống, います, 住みます, あげます, もらいます, くれます', kanji: '家族, 父, 母, 兄, 姉, 弟, 妹, 住',
  },
  {
    id: 9, topic: 'Sở thích', title: '好きなこと', subtitle: '25 đoạn đọc về sở thích, tần suất, khả năng và cuối tuần.', summary: 'Bài đọc luyện ことです, trạng từ tần suất, できます và chuỗi hành động.', titles: ['Tôi thích âm nhạc', 'Tôi thích hát', 'Cuối tuần', 'Xem phim', 'Đi biển', 'Chuyến du lịch', 'Bạn nước ngoài', 'Học ngoại ngữ', 'Thư viện', 'Đọc sách', 'Chơi bóng đá', 'Chơi bóng chuyền', 'Lái xe', 'Gia đình đi du lịch', 'Nhật Bản', 'Việt Nam', 'Điều tôi thích', 'Ca sĩ yêu thích', 'Mùa hè', 'Mùa đông', 'Bài hát', 'Chuyến đi đầu tiên', 'Ngày nghỉ', 'Đi chơi cùng bạn', 'Sở thích của tôi'],
    texts: [
      ['私の趣味は音楽を聞くことです。', 'Sở thích của tôi là nghe nhạc.', 'watashi no shumi wa ongaku o kiku koto desu.'], ['私は歌を歌うことができます。', 'Tôi có thể hát.', 'watashi wa uta o utau koto ga dekimasu.'], ['週末、友達と映画を見ます。', 'Cuối tuần tôi xem phim với bạn.', 'shuumatsu, tomodachi to eiga o mimasu.'], ['私はよく映画を見ます。', 'Tôi thường xem phim.', 'watashi wa yoku eiga o mimasu.'], ['夏、海へ行きます。', 'Mùa hè tôi đi biển.', 'natsu, umi e ikimasu.'], ['旅行が好きです。', 'Tôi thích du lịch.', 'ryokou ga suki desu.'], ['外国人の友達と日本語を話します。', 'Tôi nói tiếng Nhật với bạn nước ngoài.', 'gaikokujin no tomodachi to nihongo o hanashimasu.'], ['毎日、日本語を勉強します。', 'Mỗi ngày tôi học tiếng Nhật.', 'mainichi, nihongo o benkyou shimasu.'], ['図書館で本を読みます。', 'Tôi đọc sách ở thư viện.', 'toshokan de hon o yomimasu.'], ['小説を読むことが好きです。', 'Tôi thích đọc tiểu thuyết.', 'shousetsu o yomu koto ga suki desu.'], ['私はサッカーができます。', 'Tôi biết chơi bóng đá.', 'watashi wa sakkaa ga dekimasu.'], ['友達とテニスをします。', 'Tôi chơi tennis với bạn.', 'tomodachi to tenisu o shimasu.'], ['父は車を運転することができます。', 'Bố tôi có thể lái xe.', 'chichi wa kuruma o unten suru koto ga dekimasu.'], ['家族で旅行へ行きます。', 'Gia đình tôi đi du lịch.', 'kazoku de ryokou e ikimasu.'], ['日本料理を食べて、日本語を勉強します。', 'Tôi ăn món Nhật rồi học tiếng Nhật.', 'nihon ryouri o tabete, nihongo o benkyou shimasu.'], ['ベトナムの料理が好きです。', 'Tôi thích món ăn Việt Nam.', 'betonamu no ryouri ga suki desu.'], ['私は特に読書が好きです。', 'Tôi đặc biệt thích đọc sách.', 'watashi wa toku ni dokusho ga suki desu.'], ['好きな歌手の歌を聞きます。', 'Tôi nghe bài hát của ca sĩ yêu thích.', 'suki na kashu no uta o kikimasu.'], ['夏は水泳をします。', 'Mùa hè tôi bơi.', 'natsu wa suiei o shimasu.'], ['冬はスキーができます。', 'Mùa đông tôi có thể trượt tuyết.', 'fuyu wa sukii ga dekimasu.'], ['この歌はとてもおもしろいです。', 'Bài hát này rất thú vị.', 'kono uta wa totemo omoshiroi desu.'], ['初めて日本へ旅行しました。', 'Lần đầu tôi đã đi du lịch Nhật.', 'hajimete nihon e ryokou shimashita.'], ['休みの日、料理をしたり本を読んだりします。', 'Ngày nghỉ tôi nấu ăn và đọc sách.', 'yasumi no hi, ryouri o shitari hon o yondari shimasu.'], ['友達と公園へ行って、話します。', 'Tôi đi công viên và nói chuyện với bạn.', 'tomodachi to kouen e itte, hanashimasu.'], ['私の趣味は写真を撮ることです。', 'Sở thích của tôi là chụp ảnh.', 'watashi no shumi wa shashin o toru koto desu.'],
    ], grammar: 'JPD133-L09-G001 đến G008', vocab: '趣味, 音楽, 読書, 旅行, スポーツ, いつも, よく, ときどき, できます, どうやって', kanji: '趣味, 音楽, 旅行, 読, 作',
  },
  {
    id: 10, topic: 'Địa điểm và đường đi', title: 'バスツアー', subtitle: '25 đoạn đọc về đường đi, địa điểm, xin phép và tình huống.', summary: 'Bài đọc luyện もう, まだ, てもいい, ないでください, 見えます và なります.', titles: ['Nhà ga', 'Xe buýt', 'Taxi', 'Hỏi đường', 'Bản đồ', 'Thư viện', 'Công viên', 'Rạp phim', 'Thành phố', 'Siêu thị', 'Nhà hàng', 'Khách sạn', 'Bưu điện', 'Bệnh viện', 'Nhà sách', 'Trường học', 'Bus Tour', 'Đi bộ', 'Xe đạp', 'Đi mua sắm', 'Đường về nhà', 'Tham quan', 'Một ngày ở Tokyo', 'Chuyến xe', 'Đường đi đến trường'],
    texts: [
      ['駅はどこですか。', 'Nhà ga ở đâu?', 'eki wa doko desu ka.'], ['駅までバスで行きます。', 'Tôi đi xe buýt đến ga.', 'eki made basu de ikimasu.'], ['タクシーでホテルへ行きます。', 'Tôi đi taxi đến khách sạn.', 'takushii de hoteru e ikimasu.'], ['道を聞いて、駅へ行きます。', 'Tôi hỏi đường rồi đi đến ga.', 'michi o kiite, eki e ikimasu.'], ['地図を見て、道を探します。', 'Tôi xem bản đồ và tìm đường.', 'chizu o mite, michi o sagashimasu.'], ['図書館で本を読むことができます。', 'Có thể đọc sách ở thư viện.', 'toshokan de hon o yomu koto ga dekimasu.'], ['公園で子どもが遊んでいます。', 'Trẻ em đang chơi ở công viên.', 'kouen de kodomo ga asondeimasu.'], ['映画館で映画を見ます。', 'Tôi xem phim ở rạp.', 'eigakan de eiga o mimasu.'], ['町は大きくて、にぎやかです。', 'Thành phố lớn và nhộn nhịp.', 'machi wa ookikute, nigiyaka desu.'], ['スーパーで買い物ができます。', 'Có thể mua sắm ở siêu thị.', 'suupaa de kaimono ga dekimasu.'], ['レストランで食事をします。', 'Tôi dùng bữa ở nhà hàng.', 'resutoran de shokuji o shimasu.'], ['ホテルに泊まります。', 'Tôi ở khách sạn.', 'hoteru ni tomarimasu.'], ['郵便局で手紙を送ります。', 'Tôi gửi thư ở bưu điện.', 'yuubinkyoku de tegami o okurimasu.'], ['病院で薬をもらいます。', 'Tôi nhận thuốc ở bệnh viện.', 'byouin de kusuri o moraimasu.'], ['本屋で辞書を買います。', 'Tôi mua từ điển ở hiệu sách.', 'honya de jisho o kaimasu.'], ['学校へ歩いて行きます。', 'Tôi đi bộ đến trường.', 'gakkou e aruite ikimasu.'], ['バスツアーでいろいろな町へ行きます。', 'Tôi đi nhiều thành phố bằng tour xe buýt.', 'basu tsuaa de iroiro na machi e ikimasu.'], ['駅まで歩いて行きます。', 'Tôi đi bộ đến ga.', 'eki made aruite ikimasu.'], ['自転車で公園へ行きます。', 'Tôi đi xe đạp đến công viên.', 'jitensha de kouen e ikimasu.'], ['デパートで買い物をします。', 'Tôi mua sắm ở trung tâm thương mại.', 'depaato de kaimono o shimasu.'], ['家へ帰る道は静かです。', 'Con đường về nhà yên tĩnh.', 'ie e kaeru michi wa shizuka desu.'], ['東京タワーが見えます。', 'Có thể nhìn thấy Tháp Tokyo.', 'toukyou tawaa ga miemasu.'], ['東京で一日観光をします。', 'Tôi tham quan Tokyo một ngày.', 'toukyou de ichinichi kankou o shimasu.'], ['もうバスが来ました。', 'Xe buýt đã đến rồi.', 'mou basu ga kimashita.'], ['ここに荷物を置いてもいいですか。', 'Tôi để hành lý ở đây được không?', 'koko ni nimotsu o oite mo ii desu ka.'],
    ], grammar: 'JPD133-L10-G001 đến G014', vocab: '駅, バス, 道, 交差点, 橋, 見えます, 聞こえます, てもいいですか, ないでください', kanji: '駅, 道, 橋, 右, 左, 見',
  },
  {
    id: 11, topic: 'Sinh hoạt hằng ngày', title: '私の生活', subtitle: '25 đoạn đọc về thói quen, quá khứ và hội thoại thân mật.', summary: 'Bài đọc luyện Vています, Vたり, とき và câu hỏi thân mật.', titles: ['Một ngày của tôi', 'Buổi sáng', 'Ăn sáng', 'Đến trường', 'Giờ học', 'Giờ nghỉ', 'Ăn trưa', 'Học ở thư viện', 'Làm bài tập', 'Làm thêm', 'Về nhà', 'Nấu ăn', 'Giặt quần áo', 'Dọn phòng', 'Đi siêu thị', 'Tập thể dục', 'Chạy bộ', 'Nghe nhạc', 'Xem TV', 'Tắm', 'Đọc sách', 'Học tiếng Nhật', 'Chuẩn bị bài', 'Đi ngủ', 'Một ngày bận rộn'],
    texts: [
      ['私は毎日、忙しく生活しています。', 'Mỗi ngày tôi sống bận rộn.', 'watashi wa mainichi, isogashiku seikatsu shiteimasu.'], ['毎朝、六時に起きます。', 'Mỗi sáng tôi thức dậy lúc sáu giờ.', 'mai asa, rokuji ni okimasu.'], ['朝ご飯を食べて、学校へ行きます。', 'Tôi ăn sáng rồi đi học.', 'asagohan o tabete, gakkou e ikimasu.'], ['毎日、電車で学校へ通っています。', 'Mỗi ngày tôi đi tàu đến trường.', 'mainichi, densha de gakkou e kayotteimasu.'], ['学校で日本語を勉強しています。', 'Tôi đang học tiếng Nhật ở trường.', 'gakkou de nihongo o benkyou shiteimasu.'], ['休み時間に友達と話します。', 'Giờ nghỉ tôi nói chuyện với bạn.', 'yasumi jikan ni tomodachi to hanashimasu.'], ['昼ごはんは食堂で食べます。', 'Tôi ăn trưa ở nhà ăn.', 'hirugohan wa shokudou de tabemasu.'], ['図書館で本を読んでいます。', 'Tôi đang đọc sách ở thư viện.', 'toshokan de hon o yondeimasu.'], ['家で宿題をします。', 'Tôi làm bài tập ở nhà.', 'ie de shukudai o shimasu.'], ['夕方、アルバイトをしています。', 'Buổi tối tôi làm thêm.', 'yuugata, arubaito o shiteimasu.'], ['学校が終わって、家へ帰ります。', 'Tan học tôi về nhà.', 'gakkou ga owatte, ie e kaerimasu.'], ['母と料理を作ります。', 'Tôi nấu ăn với mẹ.', 'haha to ryouri o tsukurimasu.'], ['週末、服を洗濯します。', 'Cuối tuần tôi giặt quần áo.', 'shuumatsu, fuku o sentaku shimasu.'], ['部屋を掃除して、窓を開けます。', 'Tôi dọn phòng rồi mở cửa sổ.', 'heya o souji shite, mado o akemasu.'], ['スーパーで晩ご飯を買います。', 'Tôi mua bữa tối ở siêu thị.', 'suupaa de bangohan o kaimasu.'], ['毎週、運動しています。', 'Mỗi tuần tôi tập thể dục.', 'maishuu, undou shiteimasu.'], ['朝、公園でジョギングをします。', 'Buổi sáng tôi chạy bộ ở công viên.', 'asa, kouen de jogingu o shimasu.'], ['夜、音楽を聞いています。', 'Buổi tối tôi nghe nhạc.', 'yoru, ongaku o kiiteimasu.'], ['晩ご飯のあとでテレビを見ます。', 'Sau bữa tối tôi xem TV.', 'bangohan no ato de terebi o mimasu.'], ['夜、シャワーを浴びます。', 'Buổi tối tôi tắm vòi sen.', 'yoru, shawaa o abimasu.'], ['寝るとき、本を読みます。', 'Khi đi ngủ tôi đọc sách.', 'neru toki, hon o yomimasu.'], ['毎晩、日本語を勉強しています。', 'Tối nào tôi cũng học tiếng Nhật.', 'maiban, nihongo o benkyou shiteimasu.'], ['学校へ行くとき、宿題を準備します。', 'Khi đi học tôi chuẩn bị bài tập.', 'gakkou e iku toki, shukudai o junbi shimasu.'], ['十一時に寝ます。', 'Tôi đi ngủ lúc mười một giờ.', 'juuichi ji ni nemasu.'], ['忙しいですが、毎日楽しいです。', 'Tuy bận nhưng mỗi ngày đều vui.', 'isogashii desu ga, mainichi tanoshii desu.'],
    ], grammar: 'JPD133-L11-G001 đến G017', vocab: '毎日, 毎朝, 生活, 学校, 宿題, 休みます, 読みます, 勉強します, とき', kanji: '生, 活, 毎, 日, 学, 校, 休, 読',
  },
];

const toMarkdown = (lesson) => {
  const out = [`# JPD133 Reading Bài ${lesson.id} – ${lesson.title}`, '', `> Chủ đề: ${lesson.topic}`, `> Mục tiêu: ${lesson.subtitle}`, `> Từ vựng bắt buộc: ${lesson.vocab}`, `> Kanji bắt buộc: ${lesson.kanji}`, `> Ngữ pháp bắt buộc: ${lesson.grammar}`, ''];
  lesson.texts.forEach(([jp, vi, romaji], index) => {
    out.push(`## R${String(index + 1).padStart(3, '0')} – ${lesson.titles[index] ?? lesson.titles[0]}`, '', '### Đoạn văn tiếng Nhật', '```text', jp, '```', '', '### Romaji', '```text', romaji, '```', '', '### Dịch tiếng Việt', vi, '', '### Tóm tắt và ghi chú luyện tập', `Luyện đọc đoạn ${index + 1} theo chủ đề ${lesson.topic}; chú ý mẫu ${lesson.grammar}.`, '');
  });
  return `${out.join('\n')}\n`;
};

const supportingSentences = {
  8: [
    ['家の近くに公園があります。', 'Gần nhà có công viên.', 'ie no chikaku ni kouen ga arimasu.'],
    ['家族はみんな元気です。', 'Mọi người trong gia đình đều khỏe.', 'kazoku wa minna genki desu.'],
    ['父は会社で働いています。', 'Bố làm việc ở công ty.', 'chichi wa kaisha de hataraiteimasu.'],
    ['母は料理が上手です。', 'Mẹ nấu ăn giỏi.', 'haha wa ryouri ga jouzu desu.'],
    ['私は家族と一緒に晩ご飯を食べます。', 'Tôi ăn tối cùng gia đình.', 'watashi wa kazoku to issho ni bangohan o tabemasu.'],
    ['休みの日は家でゆっくりします。', 'Ngày nghỉ tôi nghỉ ngơi ở nhà.', 'yasumi no hi wa ie de yukkuri shimasu.'],
    ['友達からメールをもらいました。', 'Tôi nhận được email từ bạn.', 'tomodachi kara meeru o moraimashita.'],
    ['私は大切な人にプレゼントをあげます。', 'Tôi tặng quà cho người quan trọng.', 'watashi wa taisetsu na hito ni purezento o agemasu.'],
    ['家族と住んでいるので、毎日楽しいです。', 'Vì sống cùng gia đình nên mỗi ngày đều vui.', 'kazoku to sundeiru node, mainichi tanoshii desu.'],
  ],
  9: [
    ['私はよく音楽を聞きます。', 'Tôi thường nghe nhạc.', 'watashi wa yoku ongaku o kikimasu.'],
    ['ときどき友達と映画を見ます。', 'Thỉnh thoảng tôi xem phim với bạn.', 'tokidoki tomodachi to eiga o mimasu.'],
    ['一週間に二回、図書館へ行きます。', 'Một tuần tôi đi thư viện hai lần.', 'isshuukan ni nikai, toshokan e ikimasu.'],
    ['本を読むことが好きです。', 'Tôi thích đọc sách.', 'hon o yomu koto ga suki desu.'],
    ['料理を作ることができます。', 'Tôi có thể nấu ăn.', 'ryouri o tsukuru koto ga dekimasu.'],
    ['全然スポーツをしない日もあります。', 'Cũng có những ngày tôi hoàn toàn không chơi thể thao.', 'zenzen supootsu o shinai hi mo arimasu.'],
    ['でも、週末は外へ出かけます。', 'Nhưng cuối tuần tôi đi ra ngoài.', 'demo, shuumatsu wa soto e dekakemasu.'],
    ['友達と話して、写真を撮ります。', 'Tôi nói chuyện và chụp ảnh với bạn.', 'tomodachi to hanashite, shashin o torimasu.'],
    ['好きなことをすると、楽しいです。', 'Làm điều mình thích thì rất vui.', 'suki na koto o suru to, tanoshii desu.'],
  ],
  10: [
    ['駅までバスで行きます。', 'Tôi đi xe buýt đến ga.', 'eki made basu de ikimasu.'],
    ['道をまっすぐ行ってください。', 'Hãy đi thẳng theo đường.', 'michi o massugu itte kudasai.'],
    ['交差点を右に曲がります。', 'Tôi rẽ phải ở ngã tư.', 'kousaten o migi ni magarimasu.'],
    ['ここから山が見えます。', 'Từ đây có thể nhìn thấy núi.', 'koko kara yama ga miemasu.'],
    ['鳥の声が聞こえます。', 'Có thể nghe tiếng chim.', 'tori no koe ga kikoemasu.'],
    ['ここで写真を撮ってもいいですか。', 'Tôi chụp ảnh ở đây được không?', 'koko de shashin o totte mo ii desu ka.'],
    ['そこに入らないでください。', 'Xin đừng vào đó.', 'soko ni hairanaide kudasai.'],
    ['もう昼ご飯を食べました。', 'Tôi đã ăn trưa rồi.', 'mou hirugohan o tabemashita.'],
    ['まだ宿題をしていません。', 'Tôi vẫn chưa làm bài tập.', 'mada shukudai o shiteimasen.'],
  ],
  11: [
    ['毎朝、七時に起きます。', 'Mỗi sáng tôi thức dậy lúc bảy giờ.', 'mai asa, shichiji ni okimasu.'],
    ['朝ご飯を食べて、学校へ行きます。', 'Tôi ăn sáng rồi đi học.', 'asagohan o tabete, gakkou e ikimasu.'],
    ['学校で日本語を勉強しています。', 'Tôi đang học tiếng Nhật ở trường.', 'gakkou de nihongo o benkyou shiteimasu.'],
    ['休み時間に友達と話します。', 'Giờ nghỉ tôi nói chuyện với bạn.', 'yasumi jikan ni tomodachi to hanashimasu.'],
    ['家に帰ってから、晩ご飯を食べます。', 'Sau khi về nhà, tôi ăn tối.', 'ie ni kaette kara, bangohan o tabemasu.'],
    ['休みの日は本を読んだり、音楽を聞いたりします。', 'Ngày nghỉ tôi đọc sách và nghe nhạc.', 'yasumi no hi wa hon o yondari, ongaku o kiitari shimasu.'],
    ['疲れたとき、少し休みます。', 'Khi mệt, tôi nghỉ một chút.', 'tsukareta toki, sukoshi yasumimasu.'],
    ['子どものとき、よく公園で遊びました。', 'Khi còn nhỏ, tôi thường chơi ở công viên.', 'kodomo no toki, yoku kouen de asobimashita.'],
    ['友達と「映画を見ない？」と話します。', 'Tôi nói với bạn: “Xem phim không?”.', 'tomodachi to eiga o minai to hanashimasu.'],
  ],
};

const readingLessons = lessons.flatMap((lesson) => lesson.texts.map(([jp, vi, romaji], index) => ({
  id: lesson.id * 100 + index + 1,
  orderIndex: index + 1,
  topic: lesson.topic,
  title: `R${String(index + 1).padStart(3, '0')} - ${lesson.titles[index]}`,
  subtitle: `${lesson.topic}: ${lesson.titles[index]}`,
  summary: lesson.summary,
  lessonType: 'reading',
  sentences: [{ jp, vi, romaji }, ...supportingSentences[lesson.id].map(([supportingJp, supportingVi, supportingRomaji]) => ({ jp: supportingJp, vi: supportingVi, romaji: supportingRomaji }))],
})));

const importFile = {
  courseCode: 'jpd133', accessTier: 'premium', packageCode: 'speaking_jpd133', lessons: readingLessons,
};
await mkdir(path.join(root, 'server/JPLearn.Infrastructure/Data/Imports/speaking'), { recursive: true });
await writeFile(path.join(root, 'server/JPLearn.Infrastructure/Data/Imports/speaking/jpd133.lessons.json'), `${JSON.stringify(importFile, null, 2)}\n`, 'utf8');
await mkdir(path.join(root, 'docs/JPD133/Resource/JPD133_LuyệnDọcĐoạnVawn'), { recursive: true });
const lessonMarkdown = (lesson) => lesson.texts.map(([jp, vi, romaji], index) => toMarkdown({ ...lesson, titles: [lesson.titles[index]], texts: [[jp, vi, romaji], ...supportingSentences[lesson.id]] })).join('\n');
for (const lesson of lessons) await writeFile(path.join(root, `docs/JPD133/Resource/JPD133_LuyệnDọcĐoạnVawn/JPD133_Reading_Bai${lesson.id}.md`), lessonMarkdown(lesson), 'utf8');
await writeFile(path.join(root, 'docs/JPD133/Resource/JPD133_LuyệnDọcĐoạnVawn/JPD133_Reading_Full.md'), lessons.map(lessonMarkdown).join('\n---\n\n'), 'utf8');
console.log('Generated 100 JPD133 reading passages: 25 per lesson.');
