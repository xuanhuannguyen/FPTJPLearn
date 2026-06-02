import fs from 'fs';
import path from 'path';

const workspaceRoot = "b:\\FPT\\Project\\FPT_JPD";
const cotranhPath = path.join(workspaceRoot, 'material', 'Luyện Nói', 'Vấn đáp JPTJPLearn', 'JPD123', 'Bai 6', 'CoTranh.md');
const khongtranhPath = path.join(workspaceRoot, 'material', 'Luyện Nói', 'Vấn đáp JPTJPLearn', 'JPD123', 'Bai 6', 'KhongTranh.md');

const destNoImgPath = path.join(workspaceRoot, 'client', 'public', 'data', 'speaking', 'jpd123', 'qa', 'lesson6_no_image.json');
const destWithImgPath = path.join(workspaceRoot, 'client', 'public', 'data', 'speaking', 'jpd123', 'qa', 'lesson6_with_image.json');

// Kanji to Hiragana/Katakana dictionary for Lesson 6
const dictionary = {
  // Dates, Days, and Times
  "7月24日": "しchじがつにじゅうよっか", // wait, "しch" is a typo! It should be "しちがつにじゅうよっか"
  "7月24日": "しちがつにじゅうよっか",
  "6月20日": "ろくがつはつか",
  "日曜日": "にちようび",
  "にちようびに会います": "にちようびにあいます",
  "月曜日": "げつようび",
  "火曜日": "かようび",
  "水曜日": "すいようび",
  "木曜日": "もくようび",
  "金曜日": "きんようび",
  "土曜日": "どようび",
  "曜日": "ようび",

  "7時スタート": "しちじスタート",
  "7時にはじまります": "しちじにはじまります",
  "4時スタート": "よじスタート",
  "4시에 会います": "よじにあいます",
  "4時に会います": "よじにあいます",
  "4時はどうですか": "よじはどうですか",
  "4時に会いましょう": "よじにあいましょう",
  "5時はどうですか": "ごじはどうですか",
  "5時に駅で会います": "ごじにえきであいます",
  "午後": "ごご",

  "何曜日に": "なんようびに",
  "何曜日": "なんようび",
  "何月が": "なんがつが",
  "何月": "なんがつ",
  "何時に": "なんじに",
  "何時": "なんじ",
  
  "20日": "はつか",
  "24日": "にじゅうよっか",
  "6月": "ろくがつ",
  "7月": "しちがつ",
  
  // Specific Proper Names & Event Names
  "横浜ドームの野球試合": "よこはまドーム của やきゅうしあい",
  "横浜ドームの野球試合": "よこはまドームのやきゅうしあい",
  "横浜ドーム": "よこはまドーム",
  "ディズニーランドへ行きます": "ディズニーランドへいきます",
  "メガ映画館で会います": "メガえいがかんであいます",
  "メガえいがかんへ会います": "メガえいがかんへあいます",
  "メガえいがかんで会います": "メガえいがかんであいます",
  "メガえいがかん": "メガえいがかん",
  "箱根花火": "はこねはなび",
  "箱根": "はこね",
  "花火": "はなび",
  "野球試合": "やきゅうしあい",
  "野球": "やきゅう",
  "試合": "しあい",

  // Location/Meeting terms
  "場所にお時間で会います": "ばしょにおじかんであいます",
  "場所に時間で会います": "ばしょにじかんであいます",
  "待ち合わせの予定": "まちあわせのよてい",
  "待ち合わせ": "まちあわせ",
  "予定": "よてい",
  "場所へ": "ばしょへ",
  "場所は": "ばしょha",
  "場所は": "ばしょは",
  "場所で": "ばしょで",
  "場所": "ばしょ",
  "時間": "じかん",
  "日本": "にほん",
  "駅で": "えきで",
  "駅は": "えきは",
  "駅": "えき",

  // Foods, Drinks & Nature
  "ビール・コーラ・おちゃ・ジュース・水": "ビール・コーラ・おちゃ・ジュース・みず",
  "夏の飲み物": "なつののみもの",
  "飲み物": "のみもの",
  "食べ物": "たべもの",
  "水": "みず",
  "肉と魚": "にくとさかな",
  "肉のほうがすきです": "にくのほうがすきです",
  "肉がほうがすきです": "にくのほうがすきです", // Handle original document typo
  "肉より": "にくより",
  "肉と": "にくと",
  "肉の": "にくの",
  "肉が": "にくが",
  "肉": "にく",
  "魚": "さかな",
  "飛行機と新幹線": "ひこうきとしんかんせん",
  "飛行機": "ひこうき",
  "新幹線": "しんかんせん",

  // People & Preferences
  "この人は": "このひとは",
  "この人": "このひと",
  "好きな": "すきな",
  "すき": "すき",
  "人": "ひと",

  // General vocabulary
  "一年で": "いちねんで",
  "一年": "いちねん",
  "一年中": "いちねんじゅう",
  "一番": "いちばん",
  "いちばん": "いちばん",

  // Verbs & Conjugations (longest to shortest)
  "見 slippery 行きませんか": "みにいきませんか",
  "見に行きませんか": "みにいきませんか",
  "見に行きます": "みにいきます",
  "見ましたか": "みましたか",
  "見ました": "みました",
  "見に": "みに",
  "見ます": "みます",
  "見": "み",

  "食べませんでした": "たべませんでした",
  "食べていません": "たべていません",
  "食べましたか": "たべましたか",
  "食べませんか": "たべませんか",
  "食べましょう": "たべましょう",
  "食べました": "たべました",
  "食べます": "たべます",
  "食": "た",

  "あそびに行きます": "あそびにいきます",
  "遊ぶ": "あそぶ",
  "遊": "あそ",

  "まだ行っていません": "まだいっていません",
  "行っていません": "いっていません",
  "行きていません": "いっていません", // Handle document typo
  "行きませんでした": "いきませんでした",
  "行きましたか": "いきましたか",
  "行きませんか": "いきませんか",
  "行きましょう": "いきましょう",
  "行きません": "いきません",
  "行きました": "いきました",
  "行きますか": "いきますか",
  "行きます": "いきます",
  "行って": "いって",
  "行く": "いく",
  "行": "い",

  "会いましょう": "あいましょう",
  "会いますか": "あいますか",
  "会います": "あいます",
  "会い": "あい",
  "会": "あ",

  "待ちます": "まちます",
  "待って": "まって",
  "待": "ま",

  "何ですか": "なんですか",
  "何が": "なにが",
  "何を": "なにを",
  "何": "なに",

  // Single Character Fallbacks & Specific values
  "４時": "よじ",
  "５時": "ごじ",
  "７時": "しちじ",
  "4時": "よじ",
  "5時": "ごじ",
  "7時": "しちじ",
  "時": "じ",
  "映画館": "えいがかん",
  "映画": "えいが",
  "映": "えい",
  "画": "が",
  "館": "かん",
  "その日": "そのひ",
  "日": "にち",
  "月": "げつ",
  "年": "ねん",
  "一": "いち",
  "会": "あ",
  "好": "す",
  "駅": "えき",
  "夏": "なつ",
  "国": "くに",
  "本": "ほん",
  "車": "くるま",
  "山": "やま"
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
  // --- 1. Process no_image (KhongTranh.md) ---
  console.log('Processing KhongTranh.md (no_image)...');
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

  // --- 2. Process CoTranh.md (with_image) ---
  console.log('Processing CoTranh.md (with_image)...');
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
        id: q.questionId || q.id,
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
      imageUrl: `/data/speaking/jpd123/qa/lesson6_tranh${idx + 1}.png`,
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
      shortSummary: cotranhRaw.lessonOverview?.shortSummary || "Sau Bài 6 phần có tranh, sinh viên có thể nhìn tranh và trả lời câu hỏi về lời mời, so sánh hơn, so sánh nhất và cuộc hẹn.",
      studentCanDo: cotranhRaw.lessonOverview?.studentCanDo || [
        "Nhìn tranh sự kiện và nói có sự kiện gì ở đâu.",
        "Rủ bạn tham gia hoạt động trong tranh.",
        "Nhận lời hoặc từ chối lời mời lịch sự.",
        "Hỏi và trả lời về sở thích nhất trong một nhóm.",
        "So sánh hai đối tượng bằng どちら và のほうが.",
        "So sánh hơn bằngより.",
        "Hỏi trải nghiệm đã làm chưa bằng もうVましたか.",
        "Chốt thời gian và địa điểm gặp."
      ],
      mainSkills: [
        "Nghe hiểu câu hỏi mời mọc, so sánh, cuộc hẹn.",
        "Từ chối lời mời một cách tự nhiên và lịch sự.",
        "Dùng cấu pháp so sánh hơn và so sánh nhất để trả lời."
      ],
      mainGrammarFocus: cotranhRaw.lessonOverview?.mainGrammarFocus || (cotranhRaw.grammarBank ? cotranhRaw.grammarBank.map(g => g.pattern) : []),
      examTipSummary: "Khi gặp câu hỏi Bài 6 vấn đáp có tranh, hãy chú ý nghe mẫu câu mời Vませんか hay câu hỏi so sánh どちらが, いちばん để dùng đúng mẫu cấu trúc Nのほうが, いちばんすきです khi phản hồi."
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
  checkKanjiInObj(processedNoImage, 'lesson6_no_image.json');
  checkKanjiInObj(processedWithImage, 'lesson6_with_image.json');
  console.log(`Kanji scan complete. Found ${remainingKanjiCount} remaining Kanji warnings.`);

} catch (err) {
  console.error('Error during generation:', err);
}
