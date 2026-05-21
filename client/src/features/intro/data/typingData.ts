export type TypingItem = {
  kana: string;
  romaji: string;
  alternatives?: string[];
};

export type TypingGroup = {
  id: string;
  label: string; // E.g. "あ/a"
  items: TypingItem[];
};

export type TypingCategory = {
  title: string;
  allLabel: string;
  groups: TypingGroup[];
};

export const getTypingCategories = (script: 'hiragana' | 'katakana'): TypingCategory[] => {
  const isHira = script === 'hiragana';

  const categories: TypingCategory[] = [
    {
      title: 'Main Kana',
      allLabel: 'All Main Kana',
      groups: [
        {
          id: 'a',
          label: isHira ? 'あ/a' : 'ア/a',
          items: [
            { kana: isHira ? 'あ' : 'ア', romaji: 'a' },
            { kana: isHira ? 'い' : 'イ', romaji: 'i' },
            { kana: isHira ? 'う' : 'ウ', romaji: 'u' },
            { kana: isHira ? 'え' : 'エ', romaji: 'e' },
            { kana: isHira ? 'お' : 'オ', romaji: 'o' },
          ],
        },
        {
          id: 'ka',
          label: isHira ? 'か/ka' : 'カ/ka',
          items: [
            { kana: isHira ? 'か' : 'カ', romaji: 'ka' },
            { kana: isHira ? 'き' : 'キ', romaji: 'ki' },
            { kana: isHira ? 'く' : 'ク', romaji: 'ku' },
            { kana: isHira ? 'け' : 'ケ', romaji: 'ke' },
            { kana: isHira ? 'こ' : 'コ', romaji: 'ko' },
          ],
        },
        {
          id: 'sa',
          label: isHira ? 'さ/sa' : 'サ/sa',
          items: [
            { kana: isHira ? 'さ' : 'サ', romaji: 'sa' },
            { kana: isHira ? 'し' : 'シ', romaji: 'shi', alternatives: ['si'] },
            { kana: isHira ? 'す' : 'ス', romaji: 'su' },
            { kana: isHira ? 'せ' : 'セ', romaji: 'se' },
            { kana: isHira ? 'そ' : 'ソ', romaji: 'so' },
          ],
        },
        {
          id: 'ta',
          label: isHira ? 'た/ta' : 'タ/ta',
          items: [
            { kana: isHira ? 'た' : 'タ', romaji: 'ta' },
            { kana: isHira ? 'ち' : 'チ', romaji: 'chi', alternatives: ['ti'] },
            { kana: isHira ? 'つ' : 'ツ', romaji: 'tsu', alternatives: ['tu'] },
            { kana: isHira ? 'て' : 'テ', romaji: 'te' },
            { kana: isHira ? 'と' : 'ト', romaji: 'to' },
          ],
        },
        {
          id: 'na',
          label: isHira ? 'な/na' : 'ナ/na',
          items: [
            { kana: isHira ? 'な' : 'ナ', romaji: 'na' },
            { kana: isHira ? 'に' : 'ニ', romaji: 'ni' },
            { kana: isHira ? 'ぬ' : 'ヌ', romaji: 'nu' },
            { kana: isHira ? 'ね' : 'ネ', romaji: 'ne' },
            { kana: isHira ? 'の' : 'ノ', romaji: 'no' },
          ],
        },
        {
          id: 'ha',
          label: isHira ? 'は/ha' : 'ハ/ha',
          items: [
            { kana: isHira ? 'は' : 'ハ', romaji: 'ha' },
            { kana: isHira ? 'ひ' : 'ヒ', romaji: 'hi' },
            { kana: isHira ? 'ふ' : 'フ', romaji: 'fu', alternatives: ['hu'] },
            { kana: isHira ? 'へ' : 'ヘ', romaji: 'he' },
            { kana: isHira ? 'ほ' : 'ホ', romaji: 'ho' },
          ],
        },
        {
          id: 'ma',
          label: isHira ? 'ま/ma' : 'マ/ma',
          items: [
            { kana: isHira ? 'ま' : 'マ', romaji: 'ma' },
            { kana: isHira ? 'み' : 'ミ', romaji: 'mi' },
            { kana: isHira ? 'む' : 'ム', romaji: 'mu' },
            { kana: isHira ? 'め' : 'メ', romaji: 'me' },
            { kana: isHira ? 'も' : 'モ', romaji: 'mo' },
          ],
        },
        {
          id: 'ya',
          label: isHira ? 'や/ya' : 'ヤ/ya',
          items: [
            { kana: isHira ? 'や' : 'ヤ', romaji: 'ya' },
            { kana: isHira ? 'ゆ' : 'ユ', romaji: 'yu' },
            { kana: isHira ? 'よ' : 'ヨ', romaji: 'yo' },
          ],
        },
        {
          id: 'ra',
          label: isHira ? 'ら/ra' : 'ラ/ra',
          items: [
            { kana: isHira ? 'ら' : 'ラ', romaji: 'ra' },
            { kana: isHira ? 'り' : 'リ', romaji: 'ri' },
            { kana: isHira ? 'る' : 'ル', romaji: 'ru' },
            { kana: isHira ? 'れ' : 'レ', romaji: 're' },
            { kana: isHira ? 'ろ' : 'ロ', romaji: 'ro' },
          ],
        },
        {
          id: 'wa',
          label: isHira ? 'わ/wa' : 'ワ/wa',
          items: [
            { kana: isHira ? 'わ' : 'ワ', romaji: 'wa' },
            { kana: isHira ? 'を' : 'ヲ', romaji: 'wo' },
            { kana: isHira ? 'ん' : 'ン', romaji: 'n' },
          ],
        },
      ],
    },
    {
      title: 'Dakuten Kana',
      allLabel: 'All Dakuten Kana',
      groups: [
        {
          id: 'ga',
          label: isHira ? 'が/ga' : 'ガ/ga',
          items: [
            { kana: isHira ? 'が' : 'ガ', romaji: 'ga' },
            { kana: isHira ? 'ぎ' : 'ギ', romaji: 'gi' },
            { kana: isHira ? 'ぐ' : 'グ', romaji: 'gu' },
            { kana: isHira ? 'げ' : 'ゲ', romaji: 'ge' },
            { kana: isHira ? 'ご' : 'ゴ', romaji: 'go' },
          ],
        },
        {
          id: 'za',
          label: isHira ? 'ざ/za' : 'ザ/za',
          items: [
            { kana: isHira ? 'ざ' : 'ザ', romaji: 'za' },
            { kana: isHira ? 'じ' : 'ジ', romaji: 'ji', alternatives: ['zi'] },
            { kana: isHira ? 'ず' : 'ズ', romaji: 'zu' },
            { kana: isHira ? 'ぜ' : 'ゼ', romaji: 'ze' },
            { kana: isHira ? 'ぞ' : 'ゾ', romaji: 'zo' },
          ],
        },
        {
          id: 'da',
          label: isHira ? 'だ/da' : 'ダ/da',
          items: [
            { kana: isHira ? 'だ' : 'ダ', romaji: 'da' },
            { kana: isHira ? 'で' : 'デ', romaji: 'de' },
            { kana: isHira ? 'ど' : 'ド', romaji: 'do' },
          ],
        },
        {
          id: 'ba',
          label: isHira ? 'ば/ba' : 'バ/ba',
          items: [
            { kana: isHira ? 'ば' : 'バ', romaji: 'ba' },
            { kana: isHira ? 'び' : 'ビ', romaji: 'bi' },
            { kana: isHira ? 'ぶ' : 'ブ', romaji: 'bu' },
            { kana: isHira ? 'べ' : 'ベ', romaji: 'be' },
            { kana: isHira ? 'ぼ' : 'ボ', romaji: 'bo' },
          ],
        },
        {
          id: 'pa',
          label: isHira ? 'ぱ/pa' : 'パ/pa',
          items: [
            { kana: isHira ? 'ぱ' : 'パ', romaji: 'pa' },
            { kana: isHira ? 'ぴ' : 'ピ', romaji: 'pi' },
            { kana: isHira ? 'ぷ' : 'プ', romaji: 'pu' },
            { kana: isHira ? 'ぺ' : 'ペ', romaji: 'pe' },
            { kana: isHira ? 'ぽ' : 'ポ', romaji: 'po' },
          ],
        },
      ],
    },
    {
      title: 'Combination Kana',
      allLabel: 'All Combination Kana',
      groups: [
        {
          id: 'kya',
          label: isHira ? 'きゃ/kya' : 'キャ/kya',
          items: [
            { kana: isHira ? 'きゃ' : 'キャ', romaji: 'kya' },
            { kana: isHira ? 'きゅ' : 'キュ', romaji: 'kyu' },
            { kana: isHira ? 'きょ' : 'キョ', romaji: 'kyo' },
          ],
        },
        {
          id: 'sha',
          label: isHira ? 'しゃ/sha' : 'シャ/sha',
          items: [
            { kana: isHira ? 'しゃ' : 'シャ', romaji: 'sha', alternatives: ['sya'] },
            { kana: isHira ? 'しゅ' : 'シュ', romaji: 'shu', alternatives: ['syu'] },
            { kana: isHira ? 'しょ' : 'ショ', romaji: 'sho', alternatives: ['syo'] },
          ],
        },
        {
          id: 'cha',
          label: isHira ? 'ちゃ/cha' : 'チャ/cha',
          items: [
            { kana: isHira ? 'ちゃ' : 'チャ', romaji: 'cha', alternatives: ['tya'] },
            { kana: isHira ? 'ちゅ' : 'チュ', romaji: 'chu', alternatives: ['tyu'] },
            { kana: isHira ? 'ちょ' : 'チョ', romaji: 'cho', alternatives: ['tyo'] },
          ],
        },
        {
          id: 'nya',
          label: isHira ? 'にゃ/nya' : 'ニャ/nya',
          items: [
            { kana: isHira ? 'にゃ' : 'ニャ', romaji: 'nya' },
            { kana: isHira ? 'にゅ' : 'ニュ', romaji: 'nyu' },
            { kana: isHira ? 'にょ' : 'ニョ', romaji: 'nyo' },
          ],
        },
        {
          id: 'hya',
          label: isHira ? 'ひゃ/hya' : 'ヒャ/hya',
          items: [
            { kana: isHira ? 'ひゃ' : 'ヒャ', romaji: 'hya' },
            { kana: isHira ? 'ひゅ' : 'ヒュ', romaji: 'hyu' },
            { kana: isHira ? 'ひょ' : 'ヒョ', romaji: 'hyo' },
          ],
        },
        {
          id: 'mya',
          label: isHira ? 'みゃ/mya' : 'ミャ/mya',
          items: [
            { kana: isHira ? 'みゃ' : 'ミャ', romaji: 'mya' },
            { kana: isHira ? 'みゅ' : 'ミュ', romaji: 'myu' },
            { kana: isHira ? 'みょ' : 'ミョ', romaji: 'myo' },
          ],
        },
        {
          id: 'rya',
          label: isHira ? 'りゃ/rya' : 'リャ/rya',
          items: [
            { kana: isHira ? 'りゃ' : 'リャ', romaji: 'rya' },
            { kana: isHira ? 'りゅ' : 'リュ', romaji: 'ryu' },
            { kana: isHira ? 'りょ' : 'リョ', romaji: 'ryo' },
          ],
        },
        {
          id: 'gya',
          label: isHira ? 'ぎゃ/gya' : 'ギャ/gya',
          items: [
            { kana: isHira ? 'ぎゃ' : 'ギャ', romaji: 'gya' },
            { kana: isHira ? 'ぎゅ' : 'ギュ', romaji: 'gyu' },
            { kana: isHira ? 'ぎょ' : 'ギョ', romaji: 'gyo' },
          ],
        },
        {
          id: 'ja',
          label: isHira ? 'じゃ/ja' : 'ジャ/ja',
          items: [
            { kana: isHira ? 'じゃ' : 'ジャ', romaji: 'ja', alternatives: ['jya', 'zya'] },
            { kana: isHira ? 'じゅ' : 'ジュ', romaji: 'ju', alternatives: ['jyu', 'zyu'] },
            { kana: isHira ? 'じょ' : 'ジョ', romaji: 'jo', alternatives: ['jyo', 'zyo'] },
          ],
        },
        {
          id: 'bya',
          label: isHira ? 'びゃ/bya' : 'ビャ/bya',
          items: [
            { kana: isHira ? 'びゃ' : 'ビャ', romaji: 'bya' },
            { kana: isHira ? 'びゅ' : 'ビュ', romaji: 'byu' },
            { kana: isHira ? 'びょ' : 'ビョ', romaji: 'byo' },
          ],
        },
        {
          id: 'pya',
          label: isHira ? 'ぴゃ/pya' : 'ピャ/pya',
          items: [
            { kana: isHira ? 'ぴゃ' : 'ピャ', romaji: 'pya' },
            { kana: isHira ? 'ぴゅ' : 'ピュ', romaji: 'pyu' },
            { kana: isHira ? 'ぴょ' : 'ピョ', romaji: 'pyo' },
          ],
        },
      ],
    },
  ];

  if (!isHira) {
    categories.push({
      title: 'Foreign Sound',
      allLabel: 'All Foreign Sound',
      groups: [
        {
          id: 'she_che_je',
          label: 'シェ/she',
          items: [
            { kana: 'シェ', romaji: 'she' },
            { kana: 'チェ', romaji: 'che' },
            { kana: 'ジェ', romaji: 'je' },
          ],
        },
        {
          id: 'tsa_tse_tso',
          label: 'ツァ/tsa',
          items: [
            { kana: 'ツァ', romaji: 'tsa' },
            { kana: 'ツェ', romaji: 'tse' },
            { kana: 'ツォ', romaji: 'tso' },
          ],
        },
        {
          id: 'fa_fi_fe_fo',
          label: 'ファ/fa',
          items: [
            { kana: 'ファ', romaji: 'fa' },
            { kana: 'フィ', romaji: 'fi' },
            { kana: 'フェ', romaji: 'fe' },
            { kana: 'フォ', romaji: 'fo' },
          ],
        },
        {
          id: 'ti_tu',
          label: 'ティ/ti',
          items: [
            { kana: 'ティ', romaji: 'ti' },
            { kana: 'トゥ', romaji: 'tu' },
          ],
        },
        {
          id: 'di_du_dyu',
          label: 'ディ/di',
          items: [
            { kana: 'ディ', romaji: 'di' },
            { kana: 'ドゥ', romaji: 'du' },
            { kana: 'デュ', romaji: 'dyu' },
          ],
        },
      ],
    });
  }

  return categories;
};
