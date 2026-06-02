import fs from 'fs';
import path from 'path';

const workspaceRoot = "b:\\FPT\\Project\\FPT_JPD";
const cotranhPath = path.join(workspaceRoot, 'material', 'Luyện Nói', 'Vấn đáp JPTJPLearn', 'JPD123', 'Bài 4', 'cotranh.md');
const noImagePath = path.join(workspaceRoot, 'material', 'Luyện Nói', 'Vấn đáp JPTJPLearn', 'JPD123', 'Bài 4', 'jpd123_lesson4_no_image_full.json');

const destNoImgPath = path.join(workspaceRoot, 'client', 'public', 'data', 'speaking', 'jpd123', 'qa', 'lesson4_no_image.json');
const destWithImgPath = path.join(workspaceRoot, 'client', 'public', 'data', 'speaking', 'jpd123', 'qa', 'lesson4_with_image.json');

// Extensive dictionary for JPD123 Lesson 4 Kanji to Hiragana/Katakana
const dictionary = {
  // Days of the week
  "月曜日": "げつようび",
  "火曜日": "かようび",
  "水曜日": "すいようび",
  "木曜日": "もくようび",
  "金曜日": "きんようび",
  "土曜日": "どようび",
  "日曜日": "にちようび",
  
  // Specific time/durations with numbers (both half-width and full-width)
  "12月の天気": "じゅうにがつのてんき",
  "12月": "じゅうにがつ",
  "１２月": "じゅうにがつ",
  "12時": "じゅうにじ",
  "１２時": "じゅうにじ",
  "1時": "いちじ",
  "１時": "いちじ",
  "2時間半": "にじかんはん",
  "２時間半": "にじかんはん",
  "3時間半": "さんじかんはん",
  "３時間半": "さんじかんはん",
  "1時間半": "いちじかんはん",
  "１時間半": "いちじかんはん",
  "~時間半": "~じかんはん",
  "～時間半": "～じかんはん",
  "〜時間半": "〜じかんはん",
  "10時間": "じゅうじかん",
  "１０時間": "じゅうじかん",
  "20時間": "にじゅうじかん",
  "２０時間": "にじゅうじかん",
  "1時間": "いちじかん",
  "１時間": "いちじかん",
  "~時間": "~じかん",
  "～時間": "～じかん",
  "〜時間": "〜じかん",
  
  // Specific terms
  "新幹線": "しんかんせん",
  "図書館": "としょかん",
  "飛行機": "ひこうき",
  "天気がいい": "てんきがいい",
  "天気がわるい": "てんきがわるい",
  "天気": "てんき",
  "一年中": "いちねんじゅう",
  "有名": "ゆうめい",
  "近く": "ちかく",
  "歩いて": "あるいて",
  
  // Location/Directions
  "真ん中": "まんなか",
  "東京": "とうきょう",
  "北": "きた",
  "南": "みなみ",
  "東": "ひがし",
  "西": "にし",
  "町": "まち",
  "市": "し",
  "駅": "えき",
  "車": "くるま",
  "電車": "でんしゃ",
  "温泉": "おんせん",
  "教会": "きょうかい",
  "神社": "じんじゃ",
  "おてら": "おてら",
  "寺": "てら",
  "おしろ": "おしろ",
  "城": "しろ",
  
  // Objects & Nature
  "山": "やま",
  "川": "かわ",
  "海": "うみ",
  "本": "ほん",
  "雨": "あめ",
  "雪": "ゆき",
  "日": "ひ",
  "絵": "え",
  
  // Verbs & Grammars (Kanji prefixes/roots)
  "行きます": "いきます",
  "行き方": "いきかた",
  "行く": "いく",
  "行って": "いって",
  "行": "い",
  "見てください": "みてください",
  "見て": "みて",
  "見": "み",
  "あります": "あります",
  
  // Position
  "上": "うえ",
  "下": "した",
  "中": "なか",
  "外": "そと",
  "横": "よこ",
  
  // People
  "日本人": "にほんじん",
  "ベトナム人": "ベトナムじん",
  "韓国人": "かんこくじん",
  "中国人": "ちゅうごくじん",
  "フランス人": "フランスじん",
  "ドイツ人": "ドイツじん",
  "人": "ひと",
  
  // Adjectives (roots to handle conjugations like 高い, 高くない)
  "新しい": "あたらしい",
  "新しくない": "あたらしくない",
  "新しい": "あたらしい",
  "古くない": "ふるくない",
  "古い": "ふるい",
  "大きくない": "おおきくない",
  "大きくて": "おおきくて",
  "大きい": "おおきい",
  "小さくない": "ちいさくない",
  "小さい": "ちいさい",
  "高くない": "たかくない",
  "高い": "たかい",
  "低くない": "ひくくない",
  "低い": "ひくい",
  "多くない": "おおくない",
  "多い": "おおい",
  "少なくない": "すくなくない",
  "少ない": "すくない",
  "静か": "しずか",
  "暑くない": "あつくない",
  "暑い": "あつい",
  "寒くない": "さむくない",
  "寒い": "さむい",
  "暖かくない": "あたたかくない",
  "暖かい": "あたたかい",
  "涼しくない": "すずしくない",
  "涼しい": "すずしい",
  "温かくない": "あたたかくない",
  "温かい": "あたたかい",
  "熱くない": "あつくない",
  "熱い": "あつい",
  "冷たくない": "つめたくない",
  "冷たい": "つめたい",
  "甘くない": "あまくない",
  "甘い": "あまい",
  "辛くない": "からくない",
  "辛い": "からい",
  "苦くない": "にがくない",
  "苦い": "にがい",
  "すっぱくない": "すっぱくない",
  "すっぱい": "すっぱい",
  
  // Basic vocabulary
  "大学": "だいがく",
  "料理": "りょうり",
  "国": "くに",
  "日本": "にほん",
  "世界": "せかい",
  "アジア": "アジア",
  "何時間": "なんじかん",
  "何曜日": "なんようび",
  "何時": "なんじ",
  "何で": "なんで",
  "何が": "なにが",
  "何": "なに",
  "少し": "すこし",
  "とても": "とても",
  "あまり": "あまり",
  "緑": "みどり"
};

// Sort keys by length descending to prevent partial matching (e.g. 2時間半 replacing before 2時間)
const sortedKeys = Object.keys(dictionary).sort((a, b) => b.length - a.length);

function convertKanji(text) {
  if (typeof text !== 'string') return text;
  let result = text;
  for (const key of sortedKeys) {
    result = result.split(key).join(dictionary[key]);
  }
  return result;
}

const excludedKeys = new Set([
  'vi', 'meaning', 'notes', 'tips', 'commonMistakes', 'explanation',
  'sectionTitle', 'sectionViTitle', 'sectionGoal', 'pictureTitle',
  'purpose', 'sceneDescription', 'layout', 'visualDetails', 'textOnImage',
  'courseTitle', 'lessonTitle', 'dataPurpose', 'shortSummary', 'studentCanDo',
  'mainSkills', 'examTipSummary', 'designNote'
]);

function processObject(obj) {
  if (typeof obj === 'string') {
    return convertKanji(obj);
  } else if (Array.isArray(obj)) {
    return obj.map(processObject);
  } else if (obj && typeof obj === 'object') {
    const newObj = {};
    for (const key of Object.keys(obj)) {
      if (excludedKeys.has(key)) {
        newObj[key] = obj[key];
      } else {
        newObj[key] = processObject(obj[key]);
      }
    }
    return newObj;
  }
  return obj;
}

try {
  // --- 1. Process no_image.json ---
  console.log('Processing no_image...');
  const noImageRaw = JSON.parse(fs.readFileSync(noImagePath, 'utf8'));
  const processedNoImage = processObject(noImageRaw);
  
  // Ensure directory exists
  fs.mkdirSync(path.dirname(destNoImgPath), { recursive: true });
  fs.writeFileSync(destNoImgPath, JSON.stringify(processedNoImage, null, 2), 'utf8');
  console.log(`Wrote processed no_image JSON to: ${destNoImgPath}`);

  // --- 2. Process cotranh.md (with_image.json) ---
  console.log('Processing cotranh...');
  const cotranhRawText = fs.readFileSync(cotranhPath, 'utf8').trim();
  let cotranhJsonText = cotranhRawText;
  if (cotranhJsonText.startsWith('```json')) {
    cotranhJsonText = cotranhJsonText.slice(7);
  }
  if (cotranhJsonText.endsWith('```')) {
    cotranhJsonText = cotranhJsonText.slice(0, -3);
  }
  cotranhJsonText = cotranhJsonText.trim();
  
  const cotranhRaw = JSON.parse(cotranhJsonText);
  
  // Transform cotranh pictureSets to merge yesNoQuestions & whQuestions and flatten vocabulary
  const transformedPictureSets = cotranhRaw.pictureSets.map((set, idx) => {
    // Flatten relatedVocabulary
    const vocabList = (set.relatedVocabulary || []).map(v => ({
      word: convertKanji(v.word),
      reading: convertKanji(v.reading || v.word),
      meaning: v.meaning
    }));

    // Merge yesNoQuestions and whQuestions
    const questions = [];
    let qOrder = 1;

    if (set.yesNoQuestions) {
      set.yesNoQuestions.forEach(q => {
        questions.push({
          id: q.id,
          order: qOrder++,
          question: q.question,
          sampleAnswers: q.sampleAnswers,
          relatedVocabulary: vocabList, // Copy list to each question
          tips: set.tips || []
        });
      });
    }

    if (set.whQuestions) {
      set.whQuestions.forEach(q => {
        questions.push({
          id: q.id,
          order: qOrder++,
          question: q.question,
          sampleAnswers: q.sampleAnswers,
          relatedVocabulary: vocabList, // Copy list to each question
          tips: set.tips || []
        });
      });
    }

    return {
      pictureId: set.pictureId,
      pictureTitle: set.pictureTitle,
      imageUrl: `/data/speaking/jpd123/qa/lesson4_tranh${idx + 1}.png`, // Map image url
      questions: questions
    };
  });

  const finalWithImage = {
    courseCode: cotranhRaw.courseCode,
    lessonNumber: cotranhRaw.lessonNumber,
    lessonTitle: cotranhRaw.lessonTitle,
    questionMode: cotranhRaw.questionMode,
    dataPurpose: cotranhRaw.dataPurpose,
    lessonOverview: {
      shortSummary: cotranhRaw.sourceBasis?.mainCando || "Sau Bài 4 phần có tranh, sinh viên có thể nhìn tranh và trả lời câu hỏi về thành phố, phương tiện, thời tiết và món ăn.",
      studentCanDo: cotranhRaw.sourceBasis?.mainSkills || [
        "Nói về vị trí quốc gia/thành phố",
        "Nói về phương tiện và thời gian di chuyển",
        "Miêu tả thành phố, trường học, phòng/nhà",
        "Nói một nơi có gì bằng あります",
        "Hỏi đáp về thời tiết và món ăn bằng どうですか"
      ],
      mainGrammarFocus: cotranhRaw.grammarBank ? cotranhRaw.grammarBank.map(g => g.pattern) : [],
      examTipSummary: "Khi gặp câu hỏi có tranh JPD123 Bài 4, sinh viên cần chú ý đến la bàn chỉ hướng, các ký hiệu phương tiện và mốc thời gian, trạng thái phòng hoặc đặc điểm món ăn để trả lời đúng."
    },
    grammarBank: cotranhRaw.grammarBank || [],
    pictureSets: transformedPictureSets
  };

  const processedWithImage = processObject(finalWithImage);
  fs.writeFileSync(destWithImgPath, JSON.stringify(processedWithImage, null, 2), 'utf8');
  console.log(`Wrote processed with_image JSON to: ${destWithImgPath}`);

  // --- 3. Final Kanji Check ---
  const kanjiRegex = /[\u4e00-\u9faf]/;
  let remainingKanjiCount = 0;
  
  function checkKanjiInObj(obj, fileLabel, pathStr = '') {
    if (typeof obj === 'string') {
      if (kanjiRegex.test(obj)) {
        console.warn(`[WARNING] Remaining Kanji in ${fileLabel} at "${pathStr}": "${obj}"`);
        remainingKanjiCount++;
      }
    } else if (Array.isArray(obj)) {
      obj.forEach((item, idx) => checkKanjiInObj(item, fileLabel, `${pathStr}[${idx}]`));
    } else if (obj && typeof obj === 'object') {
      for (const key of Object.keys(obj)) {
        if (!excludedKeys.has(key)) {
          checkKanjiInObj(obj[key], fileLabel, `${pathStr}.${key}`);
        }
      }
    }
  }

  console.log('\nScanning generated files for remaining Kanji...');
  checkKanjiInObj(processedNoImage, 'lesson4_no_image.json');
  checkKanjiInObj(processedWithImage, 'lesson4_with_image.json');
  console.log(`Kanji scan complete. Found ${remainingKanjiCount} remaining Kanji warnings.`);

} catch (err) {
  console.error('Error during generation:', err);
}
