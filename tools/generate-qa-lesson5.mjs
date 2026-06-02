import fs from 'fs';
import path from 'path';

const workspaceRoot = "b:\\FPT\\Project\\FPT_JPD";
const cotranhPath = path.join(workspaceRoot, 'material', 'Luyện Nói', 'Vấn đáp JPTJPLearn', 'JPD123', 'Bai5', 'cotranh.md');
const khongtranhPath = path.join(workspaceRoot, 'material', 'Luyện Nói', 'Vấn đáp JPTJPLearn', 'JPD123', 'Bai5', 'khongtranh.md');

const destNoImgPath = path.join(workspaceRoot, 'client', 'public', 'data', 'speaking', 'jpd123', 'qa', 'lesson5_no_image.json');
const destWithImgPath = path.join(workspaceRoot, 'client', 'public', 'data', 'speaking', 'jpd123', 'qa', 'lesson5_with_image.json');

// Kanji to Hiragana/Katakana mapping dictionary for Lesson 5
const dictionary = {
  // Days of the week
  "日曜日": "にちようび",
  "土曜日": "どようび",
  "月曜日": "げつようび",
  "火曜日": "かようび",
  "水曜日": "すいようび",
  "木曜日": "もくようび",
  "金曜日": "きんようび",

  // Specific multi-kanji words
  "日本語": "にほんご",
  "日本": "にほん",
  "先生": "せんせい",
  "休み": "やすみ",
  "国": "くに",
  "本": "ほん",
  "車": "くるま",
  "山": "やま",

  // Actions and verbs (sorted by specificity/length)
  "聞きませんでした": "ききませんでした",
  "聞きましたか": "ききましたか",
  "聞きました": "ききました",
  "聞きます": "ききます",
  "聞": "き",

  "見に行きます": "みにいきます",
  "見ましたか": "みましたか",
  "見ました": "みました",
  "見ます": "みます",
  "見に": "みに",
  "見": "み",

  "買いに行きます": "かいにいきます",
  "買いたくないです": "かいたくないです",
  "買いたいですか": "かいたいですか",
  "買いたいです": "かいたいです",
  "買いました": "かいました",
  "買いたい": "かいたい",
  "買います": "かいます",
  "買いに": "かいに",
  "買い": "かい",
  "買": "かい",

  "会いに行きます": "あいにいきます",
  "会いたいですか": "あいたいですか",
  "会いたいです": "あいたいです",
  "会います": "あいます",
  "会いに": "あいに",
  "会": "あ",

  "行きませんでした": "いきませんでした",
  "行きましたか": "いきましたか",
  "行きません": "いきません",
  "行きました": "いきました",
  "行きたいですか": "いきたいですか",
  "行きたいです": "いきたいです",
  "行きますか": "いきますか",
  "行きます": "いきます",
  "行って": "いって",
  "行く": "いく",
  "行": "い",

  "帰りますか": "かえりますか",
  "帰りません": "かえりません",
  "帰りました": "かえりました",
  "帰ります": "かえります",
  "帰": "かえ",

  "入りました": "はいりました",
  "入る": "はいる",
  "入": "はい",

  "遊びました": "あそびました",
  "遊ぶ": "あそぶ",
  "遊": "あそ",

  // Question words
  "何をしましたか": "なにをしましたか",
  "何を": "なにを",
  "何が": "なにが",
  "何": "なに",

  // Adjectives
  "新しい": "あたらしい",
  "新しくない": "あたらしくない",
  "新": "あたら"
};

// Sort keys by length descending to prevent partial matching
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
  // --- 1. Process no_image (khongtranh.md) ---
  console.log('Processing khongtranh.md (no_image)...');
  const khongtranhRawText = fs.readFileSync(khongtranhPath, 'utf8').trim();
  let khongtranhJsonText = khongtranhRawText;
  if (khongtranhJsonText.startsWith('```json')) {
    khongtranhJsonText = khongtranhJsonText.slice(7);
  }
  if (khongtranhJsonText.endsWith('```')) {
    khongtranhJsonText = khongtranhJsonText.slice(0, -3);
  }
  khongtranhJsonText = khongtranhJsonText.trim();
  
  const khongtranhRaw = JSON.parse(khongtranhJsonText);
  const processedNoImage = processObject(khongtranhRaw);
  
  // Ensure output directory exists
  fs.mkdirSync(path.dirname(destNoImgPath), { recursive: true });
  fs.writeFileSync(destNoImgPath, JSON.stringify(processedNoImage, null, 2), 'utf8');
  console.log(`Wrote processed no_image JSON to: ${destNoImgPath}`);

  // --- 2. Process cotranh.md (with_image) ---
  console.log('Processing cotranh.md (with_image)...');
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
  
  // Transform cotranh pictureSets to format relatedVocabulary and questions
  const transformedPictureSets = cotranhRaw.pictureSets.map((set, idx) => {
    const questions = set.questions.map((q, qIdx) => {
      // Format question relatedVocabulary to flat format
      const vocabList = (q.relatedVocabulary || []).map(v => ({
        word: convertKanji(v.word),
        reading: convertKanji(v.reading || v.word),
        meaning: v.meaning
      }));

      return {
        id: q.id,
        order: qIdx + 1,
        question: {
          ja: convertKanji(q.question.ja),
          vi: q.question.vi
        },
        sampleAnswers: (q.sampleAnswers || []).map(sa => ({
          ja: convertKanji(sa.ja),
          vi: sa.vi
        })),
        relatedVocabulary: vocabList,
        explanation: q.explanation,
        tips: q.tips || [],
        commonMistakes: q.commonMistakes || []
      };
    });

    return {
      pictureId: set.pictureId,
      pictureTitle: set.pictureTitle,
      imageUrl: `/data/speaking/jpd123/qa/lesson5_tranh${idx + 1}.png`,
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
      shortSummary: cotranhRaw.lessonOverview?.shortSummary || "Sau Bài 5 phần có tranh, sinh viên có thể nhìn tranh và trả lời câu hỏi về quá khứ, cảm nhận, sở thích và mong muốn.",
      studentCanDo: cotranhRaw.lessonOverview?.studentCanDo || [
        "Hỏi đáp về việc đã làm vào ngày nghỉ.",
        "Hỏi đáp cảm nhận về hoạt động, thời tiết, bài kiểm tra, chuyến đi.",
        "Nói được mình thích gì, muốn có gì, muốn làm gì.",
        "Nói được kế hoạch sắp tới vào cuối tuần hoặc kỳ nghỉ."
      ],
      mainSkills: [
        "Nghe hiểu câu hỏi quá khứ, cảm nhận, sở thích, mong muốn.",
        "Sử dụng tính từ chia quá khứ để trả lời.",
        "Trả lời đúng dạng động từ Vました hoặc Vたいです."
      ],
      mainGrammarFocus: cotranhRaw.lessonOverview?.mainGrammarFocus || (cotranhRaw.grammarBank ? cotranhRaw.grammarBank.map(g => g.pattern) : []),
      examTipSummary: "Khi thi vấn đáp Bài 5, chú ý nghe động từ chia quá khứ ましたか hoặc tính từ quá khứ どうでしたか để chọn mẫu tương ứng. Ngoài ra cần phân biệt các mẫu ほしい (muốn có vật) và たい (muốn làm hành động)."
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
  checkKanjiInObj(processedNoImage, 'lesson5_no_image.json');
  checkKanjiInObj(processedWithImage, 'lesson5_with_image.json');
  console.log(`Kanji scan complete. Found ${remainingKanjiCount} remaining Kanji warnings.`);

} catch (err) {
  console.error('Error during generation:', err);
}
