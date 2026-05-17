export type KanaScript = 'hiragana' | 'katakana';

export type KanaItem = {
  romaji: string;
  kana: string;
  mnemonic: string;
};

export type KanaRow = {
  label: string;
  items: KanaItem[];
};

export const hiraganaRows: KanaRow[] = [
  {
    label: 'Hàng A',
    items: [
      { romaji: 'a', kana: 'あ', mnemonic: 'Quả táo (apple) - /ˈæ.pəl/' },
      { romaji: 'i', kana: 'い', mnemonic: 'Nhìn giống 2 chữ i đứng cạnh nhau' },
      { romaji: 'u', kana: 'う', mnemonic: 'Võ sĩ đấm bốc bị đấm vào bụng Cúi gập người và kêu "u"' },
      { romaji: 'e', kana: 'え', mnemonic: 'Ninja tràn đầy năng lượng (energetic ninja) /ˌen.əˈdʒet.ɪk ˈnɪn.dʒə/ - Tư thế đang lao đi vút đi' },
      { romaji: 'o', kana: 'お', mnemonic: 'Đĩa bay UFO (UFO) - Có một nét lơ lửng bên cạnh giống đĩa bay /ˌjuː.efˈəʊ/' },
    ],
  },
  {
    label: 'Hàng K',
    items: [
      { romaji: 'ka', kana: 'か', mnemonic: 'Dao dùng để  "Cắt"' },
      { romaji: 'ki', kana: 'き', mnemonic: 'Chiếc chìa khóa (key) /kiː/' },
      { romaji: 'ku', kana: 'く', mnemonic: 'Mỏ của con chim cúc cu cuckoo  /ˈkʊk.uː/' },
      { romaji: 'ke', kana: 'け', mnemonic: 'Giống chiếc thùng phi (KEG) /keɡ/' },
      { romaji: 'ko', kana: 'こ', mnemonic: 'Hai con cá Koi bơi vòng quanh nhau (koi fish) /kɔɪ fɪʃ/' },
    ],
  },
  {
    label: 'Hàng S',
    items: [
      { romaji: 'sa', kana: 'さ', mnemonic: 'Khuôn mặt buồn bã (sad face) -  /sæd feɪs/' },
      { romaji: 'shi', kana: 'し', mnemonic: 'Lưỡi câu cá (fishing hook) /ˈfɪʃ.ɪŋ hʊk/' },
      { romaji: 'su', kana: 'す', mnemonic: 'Ống hút xoắn trong cốc (spiral straw)  -/ˈspaɪ.rəl strɔː/' },
      { romaji: 'se', kana: 'せ', mnemonic: 'Khuôn mặt nhìn nghiêng đang há miệng nói "say /seɪ/ something ". :' },
      { romaji: 'so', kana: 'そ', mnemonic: 'Đường may zigzag (zigzag sewing stitch) /ˈzɪɡ.zæɡ ˈsəʊ.ɪŋ stɪtʃ/' },
    ],
  },
  {
    label: 'Hàng T',
    items: [
      { romaji: 'ta', kana: 'た', mnemonic: 'Chữ Ta' },
      { romaji: 'chi', kana: 'ち', mnemonic: 'Đội trưởng đội cổ vũ (cheerleader) - ˈtʃɪə.liː.də/' },
      { romaji: 'tsu', kana: 'つ', mnemonic: 'Cơn sóng thần (tsunami wave) /tsuːˈnɑː.mi/' },
      { romaji: 'te', kana: 'て', mnemonic: 'Đơn giản là giống chữ T " Tê"' },
      { romaji: 'to', kana: 'と', mnemonic: 'Cơn lốc xoáy (tornado) /tɔːˈneɪ.dəʊ/' },
    ],
  },
  {
    label: 'Hàng N',
    items: [
      { romaji: 'na', kana: 'な', mnemonic: 'Sợi dây/Hạt dẻ bị rối (complicated nut) /nʌt/' },
      { romaji: 'ni', kana: 'に', mnemonic: 'Đầu gối (knee) /niː/' },
      { romaji: 'nu', kana: 'ぬ', mnemonic: 'Đôi đũa đang gắp một búi mì (chopsticks holding noodles) /ˈnuː.dəlz/' },
      { romaji: 'ne', kana: 'ね', mnemonic: 'Con ốc sên trốn sau cái đinh (snail hiding behind a nail) /sneɪl/, /neɪl/' },
      { romaji: 'no', kana: 'の', mnemonic: 'Biển cấm (no sign) /nəʊ saɪn/' },
    ],
  },
  {
    label: 'Hàng H',
    items: [
      { romaji: 'ha', kana: 'は', mnemonic: 'Chữ H in hoa và chữ a viết thường' },
      { romaji: 'hi', kana: 'ひ', mnemonic: 'Khuôn mặt đang cười hi hi hi' },
      { romaji: 'fu', kana: 'ふ', mnemonic: 'Núi Phú Sĩ (Mount Fuji) ˈfuː.dʒi/' },
      { romaji: 'he', kana: 'へ', mnemonic: 'Mũi tên chỉ lên thiên đường/bầu trời (pointing up to heaven) /ˈhev.ən/' },
      { romaji: 'ho', kana: 'ほ', mnemonic: 'Mặt chú ngựa với bờm ở bên trái (horse\'s face) /hɔːs/' },
    ],
  },
  {
    label: 'Hàng M',
    items: [
      { romaji: 'ma', kana: 'ま', mnemonic: 'Người đàn ông đeo mặt nạ (man in a mask) /mæn ɪn ə mɑːsk/' },
      { romaji: 'mi', kana: 'み', mnemonic: 'Nốt nhạc Mi (musical note mi)' },
      { romaji: 'mu', kana: 'む', mnemonic: 'Con bò kêu Muuuu' },
      { romaji: 'me', kana: 'め', mnemonic: 'Đũa làm rơi mì tạo ra mớ hỗn độn (mess) /mes/' },
      { romaji: 'mo', kana: 'も', mnemonic: 'Con kỳ đà (monitor lizard) - /ˈmɒn.ɪ.tə ˈlɪz.əd/' },
    ],
  },
  {
    label: 'Hàng R',
    items: [
      { romaji: 'ra', kana: 'ら', mnemonic: 'Con thỏ ngồi bằng hai chân sau (rabbit) /ˈræb.ɪt/' },
      { romaji: 'ri', kana: 'り', mnemonic: 'Dòng sông (river) /ˈrɪv.ə/' },
      { romaji: 'ru', kana: 'る', mnemonic: 'Bàn tay cầm viên hồng ngọc (ruby) /ˈruː.bi/' },
      { romaji: 're', kana: 'れ', mnemonic: 'Con tuần lộc đang ngước nhìn lên (reindeer) /ˈreɪn.dɪə/' },
      { romaji: 'ro', kana: 'ろ', mnemonic: 'Bị cướp mất viên ngọc (robbed - ruby is gone) /rɒbd/' },
    ],
  },
  {
    label: 'Hàng Y',
    items: [
      { romaji: 'ya', kana: 'や', mnemonic: 'Bò Tây Tạng yak - /jæk/' },
      { romaji: 'yu', kana: 'ゆ', mnemonic: 'Kỳ lân ma thuật (magical unicorn) /ˈjuː.nɪ.kɔːn/' },
      { romaji: 'yo', kana: 'よ', mnemonic: 'Đồ chơi Yoyo treo lủng lẳng trên ngón tay (yoyo dangling from a finger) /ˈjəʊ.jəʊ/' },
    ],
  },
  {
    label: 'Hàng W/N',
    items: [
      { romaji: 'wa', kana: 'わ', mnemonic: 'Thiên nga trắng bơi trên mặt nước (white swan floating on the water) /waɪt swɒn/' },
      { romaji: 'wo', kana: 'を', mnemonic: 'Vết nứt trên tường (crack in the wall) /kræk ɪn ðə wɔːl/' },
      { romaji: 'n', kana: 'ん', mnemonic: 'Chữ N in nghiêng' },
    ],
  },
];

export const katakanaRows: KanaRow[] = [
  {
    label: 'Hàng A',
    items: [
      { romaji: 'a', kana: 'ア', mnemonic: 'Axe /æks/ (Cây rìu)' },
      { romaji: 'i', kana: 'イ', mnemonic: 'Easel /ˈiː.zəl/ (Giá vẽ tranh được đặt nằm ngang)' },
      { romaji: 'u', kana: 'ウ', mnemonic: 'Hình dáng là phiên bản góc cạnh hơn của chữ Hiragana \'u\'' },
      { romaji: 'e', kana: 'エ', mnemonic: 'Elevator doors /ˈel.ə.veɪ.tər dɔːrz/ (Cửa thang máy)' },
      { romaji: 'o', kana: 'オ', mnemonic: 'Opera singer /ˈɒp.ər.ə ˈsɪŋ.ər/ (Ca sĩ hát Opera đang mở to miệng)' },
    ],
  },
  {
    label: 'Hàng K',
    items: [
      { romaji: 'ka', kana: 'カ', mnemonic: 'Giống chữ Hiragana \'ka\' nhưng góc cạnh hơn và không có nét vạch thứ ba' },
      { romaji: 'ki', kana: 'キ', mnemonic: 'Giống chữ Hiragana \'ki\' nhưng không có phần nét cong ở dưới cùng' },
      { romaji: 'ku', kana: 'ク', mnemonic: 'Cuckoo\'s tail /ˈkʊk.uːz teɪl/ (Đuôi của chim cúc cu)' },
      { romaji: 'ke', kana: 'ケ', mnemonic: 'Sideways K /ˈsaɪd.weɪz keɪ/ (Chữ K nằm ngang)' },
      { romaji: 'ko', kana: 'コ', mnemonic: 'Road with two corners /roʊd wɪð tuː ˈkɔːr.nərz/ (Con đường có hai khúc cua)' },
    ],
  },
  {
    label: 'Hàng S',
    items: [
      { romaji: 'sa', kana: 'サ', mnemonic: 'Saddle /ˈsæd.əl/ (Yên ngựa)' },
      { romaji: 'shi', kana: 'シ', mnemonic: 'Sinking ship /ˈsɪŋ.kɪŋ ʃɪp/ (Con tàu đang chìm)' },
      { romaji: 'su', kana: 'ス', mnemonic: 'Person skiing /ˈpɜːr.sən ˈskiː.ɪŋ/ (Người đang trượt tuyết)' },
      { romaji: 'se', kana: 'セ', mnemonic: 'Phiên bản góc cạnh hơn của chữ Hiragana \'se\' và thiếu một phần ở góc trên bên phải' },
      { romaji: 'so', kana: 'ソ', mnemonic: 'Soft served ice cream cone /sɒft sɜːrvd aɪs kriːm koʊn/ (Cây kem ốc quế)' },
    ],
  },
  {
    label: 'Hàng T',
    items: [
      { romaji: 'ta', kana: 'タ', mnemonic: 'Tablet /ˈtæb.lət/ (Người đang cầm máy tính bảng)' },
      { romaji: 'chi', kana: 'チ', mnemonic: 'Chicken /ˈtʃɪk.ɪn/ (Con gà)' },
      { romaji: 'tsu', kana: 'ツ', mnemonic: 'Tuna fish\'s head /ˈtuː.nə fɪʃɪz hed/ (Đầu cá ngừ)' },
      { romaji: 'te', kana: 'テ', mnemonic: 'Telephone pole /ˈtel.ə.foʊn poʊl/ (Cột điện thoại bị bẻ cong ở một góc)' },
      { romaji: 'to', kana: 'ト', mnemonic: 'Temple /ˈtem.pəl/ (Góc nhìn nghiêng của một ngôi đền)' },
    ],
  },
  {
    label: 'Hàng N',
    items: [
      { romaji: 'na', kana: 'ナ', mnemonic: 'Curved knife /kɜːrvd naɪf/ (Con dao bị cong)' },
      { romaji: 'ni', kana: 'ニ', mnemonic: 'Giống chữ Hiragana \'ni\'' },
      { romaji: 'nu', kana: 'ヌ', mnemonic: 'Noose /nuːs/ (Dây thừng thòng lọng)' },
      { romaji: 'ne', kana: 'ネ', mnemonic: 'Nest in a tree /nest ɪn ə triː/ (Cái tổ trên cây)' },
      { romaji: 'no', kana: 'ノ', mnemonic: 'No sign /noʊ saɪn/ (Biển báo cấm - giống chữ Hiragana)' },
    ],
  },
  {
    label: 'Hàng H',
    items: [
      { romaji: 'ha', kana: 'ハ', mnemonic: 'Roof of a house /ruːf əv ə haʊs/ (Mái nhà)' },
      { romaji: 'hi', kana: 'ヒ', mnemonic: 'Side of heel /saɪd əv hiːl/ (Mặt bên của gót chân) - Cũng nhìn như mặt cười "HiHi"' },
      { romaji: 'fu', kana: 'フ', mnemonic: 'Tip of a foot /tɪp əv ə fʊt/ (Mũi bàn chân)' },
      { romaji: 'he', kana: 'ヘ', mnemonic: 'Hoàn toàn giống với chữ Hiragana \'he\'' },
      { romaji: 'ho', kana: 'ホ', mnemonic: 'Shining holy cross /ˈʃaɪ.nɪŋ ˈhoʊ.li krɒs/ (Cây thánh giá tỏa sáng)' },
    ],
  },
  {
    label: 'Hàng M',
    items: [
      { romaji: 'ma', kana: 'マ', mnemonic: 'Manta ray /ˈmæn.tə reɪ/ (Cá đuối)' },
      { romaji: 'mi', kana: 'ミ', mnemonic: 'Middle /ˈmɪd.əl/ (Nhìn vào đường kẻ ở giữa)' },
      { romaji: 'mu', kana: 'ム', mnemonic: 'Moose\'s antlers /ˈmuːsɪz ˈænt.lərz/ (Sừng của con nai sừng tấm)' },
      { romaji: 'me', kana: 'メ', mnemonic: 'Mail /meɪl/ nhìn như logo của email' },
      { romaji: 'mo', kana: 'モ', mnemonic: 'Rất giống chữ Hiragana \'mo\'' },
    ],
  },
  {
    label: 'Hàng R',
    items: [
      { romaji: 'ra', kana: 'ラ', mnemonic: 'Lounging chair /ˈlaʊndʒ.ɪŋ tʃer/ (Chiếc ghế dài dùng để nằm nghỉ ngơi)' },
      { romaji: 'ri', kana: 'リ', mnemonic: 'Hoàn toàn giống chữ Hiragana \'ri\'' },
      { romaji: 'ru', kana: 'ル', mnemonic: 'Roots of a tree /ruːts əv ə triː/ (Rễ cây)' },
      { romaji: 're', kana: 'レ', mnemonic: 'Razor blade /ˈreɪ.zər bleɪd/ (Lưỡi dao cạo)' },
      { romaji: 'ro', kana: 'ロ', mnemonic: 'Robot\'s mouth /ˈroʊ.bɒts maʊθ/ (Miệng của robot)' },
    ],
  },
  {
    label: 'Hàng Y',
    items: [
      { romaji: 'ya', kana: 'ヤ', mnemonic: 'Giống chữ Hiragana \'ya\'' },
      { romaji: 'yu', kana: 'ユ', mnemonic: 'U-boat /ˈjuː.boʊt/ (Kính tiềm vọng của tàu ngầm U-boat ở dưới nước)' },
      { romaji: 'yo', kana: 'ヨ', mnemonic: 'Yoke /joʊk/ (Cái ách kéo bởi hai con bò)' },
    ],
  },
  {
    label: 'Hàng W/N',
    items: [
      { romaji: 'wa', kana: 'ワ', mnemonic: 'Wine glass /waɪn ɡlæs/ (Ly rượu vang)' },
      { romaji: 'wo', kana: 'ヲ', mnemonic: 'World Olympic torch /wɜːrld əˈlɪm.pɪk tɔːrtʃ/ (Một phần của ngọn đuốc Olympic thế giới)' },
      { romaji: 'n', kana: 'ン', mnemonic: 'Ví dụ nói về tàu không gian vào (enter) vũ trụ' },
    ],
  },
];

export const getKanaRows = (script: KanaScript) => (
  script === 'hiragana' ? hiraganaRows : katakanaRows
);
