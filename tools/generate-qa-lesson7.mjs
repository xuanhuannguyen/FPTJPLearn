import fs from 'fs';
import path from 'path';

const workspaceRoot = "b:\\FPT\\Project\\FPT_JPD";
const cotranhPath = path.join(workspaceRoot, 'material', 'Luyện Nói', 'Vấn đáp JPTJPLearn', 'JPD123', 'Bai 7', 'CoTranh.md');
const khongtranhPath = path.join(workspaceRoot, 'material', 'Luyện Nói', 'Vấn đáp JPTJPLearn', 'JPD123', 'Bai 7', 'Khongtranh.md');

const destNoImgPath = path.join(workspaceRoot, 'client', 'public', 'data', 'speaking', 'jpd123', 'qa', 'lesson7_no_image.json');
const destWithImgPath = path.join(workspaceRoot, 'client', 'public', 'data', 'speaking', 'jpd123', 'qa', 'lesson7_with_image.json');

// Kanji to Hiragana/Katakana dictionary for Lesson 7
const dictionary = {
  // Phrases and longer words first to avoid partial replacements
  "となりに何がありますか": "となりになにがありますか",
  "N の となりに 動物 が いますか。": "N の となりに どうぶつ が いますか。",
  "人 は どこに いますか。": "ひと は どこに いますか。",
  "人 の うしろに": "ひと の うしろに",
  "人 が います。": "ひと が います。",
  "FPT大学": "FPTだいがく",
  "大学": "だいがく",
  "日本語": "にほんご",
  "ベトナム人": "ベトナムじん",
  "名前": "なまえ",
  "動物": "どうぶつ",
  
  "もって行ってください": "もっていってください",
  "もって行きます": "もっていきます",
  "もって来てください": "もってきてください",
  "もって来ます": "もってきます",
  
  "食べています": "たべています",
  "食べます": "たべます",
  "食べて": "たべて",
  "食べ": "たべ",
  
  "話しています": "はなしています",
  "話します": "はなします",
  "話して": "はなして",
  "話し": "はなし",
  
  "書きます": "かきます",
  "書いて": "かいて",
  "書き": "かき",
  
  "行って": "いって",
  "行き": "いき",
  
  "木": "き",
  "人": "ひと",

  // Single Character Fallbacks
  "大": "だい",
  "学": "がく",
  "日": "にち",
  "本": "ほん",
  "語": "ご",
  "名": "な",
  "前": "まえ",
  "動": "どう",
  "物": "ぶつ",
  "食": "た",
  "行": "い",
  "話": "はな",
  "書": "か"
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
  // --- 1. Process no_image (Khongtranh.md) ---
  console.log('Processing Khongtranh.md (no_image)...');
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
      imageUrl: `/data/speaking/jpd123/qa/lesson7_tranh${idx + 1}.png`,
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
      shortSummary: cotranhRaw.lessonOverview?.shortSummary || "Sau Bài 7 phần có tranh, sinh viên có thể miêu tả vị trí đồ vật, phương thức hành động và trạng thái đang làm.",
      studentCanDo: cotranhRaw.lessonOverview?.studentCanDo || [
        "Hỏi và trả lời về vị trí của địa điểm/đồ vật bằng まえ, うしろ, となり, ちかく.",
        "Hỏi và trả lời về việc làm một hành động bằng phương tiện/dụng cụ gì bằng trợ từ で.",
        "Hỏi và trả lời về việc di chuyển mang theo vật gì bằng もって行きます và もって来ます.",
        "Hỏi và trả lời về hành động đang diễn ra tại thời điểm nói bằng thể ています."
      ],
      mainSkills: [
        "Nghe hiểu các câu hỏi về vị trí, phương thức thực hiện hành động và trạng thái đang diễn ra.",
        "Trả lời trôi chảy, tự nhiên sử dụng cấu trúc ngữ pháp tương ứng.",
        "Nhìn tranh và trích xuất đúng thông tin để trả lời câu hỏi vấn đáp."
      ],
      mainGrammarFocus: cotranhRaw.lessonOverview?.mainGrammarFocus || (cotranhRaw.grammarBank ? cotranhRaw.grammarBank.map(g => g.pattern) : []),
      examTipSummary: "Khi gặp câu hỏi về vị trí (どこにありますか), hãy định vị đúng các địa điểm trên tranh. Khi nghe hỏi về phương thức (なにで...か), hãy trả lời rõ dụng cụ bằng で. Khi nghe hỏi hành động đang diễn ra (していますか), chú ý chia động từ thể ています."
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
  checkKanjiInObj(processedNoImage, 'lesson7_no_image.json');
  checkKanjiInObj(processedWithImage, 'lesson7_with_image.json');
  console.log(`Kanji scan complete. Found ${remainingKanjiCount} remaining Kanji warnings.`);

} catch (err) {
  console.error('Error during generation:', err);
}
