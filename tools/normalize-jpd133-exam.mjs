import { mkdir, readFile, writeFile } from 'node:fs/promises';
import path from 'node:path';

const root = process.cwd();
const sourceMarkdown = path.join(root, 'docs/JPD133/de-thi-jpd133.md');
const manualVisionPath = path.join(root, 'docs/JPD133/manual/jpd133-vision-verified.json');
const importPath = path.join(root, 'server/JPLearn.Infrastructure/Data/Imports/exam/jpd133.questions.json');
const auditPath = path.join(root, 'docs/JPD133/jpd133-exam-audit.json');
const normalizedMarkdownPath = path.join(root, 'docs/JPD133/de-thi-jpd133-normalized.md');

const topics = [
  { code: 'kanji', label: 'Hán tự', orderIndex: 1 },
  { code: 'grammar', label: 'Ngữ pháp', orderIndex: 2 },
  { code: 'vocabulary', label: 'Từ vựng', orderIndex: 3 },
  { code: 'conversation', label: 'Giao tiếp', orderIndex: 4 },
  { code: 'odd_one_out', label: 'Khác loại', orderIndex: 5 },
  { code: 'reading', label: 'Đọc hiểu', orderIndex: 6 },
];

const q = (id, topic, questionText, options, correct, explanation, sourceOrder, type = 'standalone') => ({
  id, type, topic, level: 'N5', questionText, explanation, orderIndex: id,
  options: options.map((text, index) => ({ label: String.fromCharCode(65 + index), text, isCorrect: index === correct })),
  sourceOrder,
});

const baseQuestions = [
  q(1, 'conversation', 'どうやって飛行機のチケットを予約しますか。', ['インターネットで予約します。', '住所を見せます。', '電話に参加します。', 'バスを降ります。'], 0, 'Đáp án A. Hỏi cách đặt vé máy bay nên câu trả lời phù hợp là đặt vé qua Internet; các lựa chọn còn lại không trả lời đúng cách thức đặt vé.', 0),
  q(2, 'grammar', '１か月に２回、料理教室に（　　　）います。', ['つくって', 'ひいて', 'かよって', 'さがして'], 2, 'Đáp án C. 「料理教室に通っています」nghĩa là đang đi học lớp nấu ăn. 「通う」chia dạng て là 「かよって」.', 1),
  q(3, 'kanji', '７時ですよ。はやく「おきて」ください。', ['始きて', '起きて', '乗きて', '歩きて'], 1, 'Đáp án B. 「おきて」trong ngữ cảnh thức dậy được viết là 「起きて」, dạng て của 「起きる」.', 2),
  q(4, 'grammar', 'ここに荷物を（　　　）いいですか。', ['およいで', 'おいても', 'おきても', 'おきて'], 1, 'Đáp án B. 「Vてもいいですか」dùng để xin phép. Động từ 「置く」chia dạng て là 「置いて」, nên đáp án là 「おいても」.', 3),
  q(5, 'kanji', '明日の「朝」、何をしますか。', ['あさ', 'ばん', 'よる', 'ひる'], 0, 'Đáp án A. 「朝」đọc là 「あさ」, nghĩa là buổi sáng.', 4),
  q(6, 'grammar', '趣味は本を（　　　）です。', ['読む', '読みます', '読むこと', '読書'], 2, 'Đáp án C. Mẫu 「趣味は Vることです」dùng để nói sở thích là làm gì đó. Vì vậy 「読むこと」là dạng đúng.', 5),
  q(7, 'kanji', 'きれいな「りょかん」ですね。', ['旅管', '族管', '旅館', '族館'], 2, 'Đáp án C. 「りょかん」được viết là 「旅館」, nghĩa là nhà trọ kiểu Nhật.', 6),
  q(8, 'grammar', '趣味は（　　　）です。', ['ゲームをします', 'ゲームをする', 'ゲームをすること', 'ゲームをしますこと'], 2, 'Đáp án C. Sau 「趣味は」dùng danh từ hóa 「Vること」. 「ゲームをすること」có nghĩa là việc chơi game.', 7),
  q(9, 'grammar', 'あそこで買い物が（　　　）。', ['払います', 'できます', '買います', '借ります'], 1, 'Đáp án B. 「買い物ができます」nghĩa là có thể mua sắm. 「できる」đi với danh từ hoạt động và trợ từ が.', 8),
  q(10, 'grammar', 'マルコさんはサッカーがよくて、（　　　）です。', ['かっこよくて', 'きれい', 'きれくて', 'かっこいい'], 0, 'Đáp án A. Tính từ い 「かっこいい」đổi sang dạng て là 「かっこよくて」để nối với 「です」ở vế sau.', 9),
  q(11, 'grammar', '子供がいますから、たばこを（　　　）ください。', ['吸って', '買わないで', '買って', '吸わないで'], 3, 'Đáp án D. 「Vないでください」dùng để yêu cầu không làm gì. 「吸う」đổi thành 「吸わないで」.', 10),
  q(12, 'grammar', '日曜日、音楽を（　　　）、小説を（　　　）します。', ['聞くこと、読むこと', '聞いたり、読んだり', '読んだり、聞いたり', '読むこと、聞くこと'], 1, 'Đáp án B. 「Vたり、Vたりします」liệt kê các hành động tiêu biểu; 「聞く」→「聞いたり」và 「読む」→「読んだり」.', 11),
  q(13, 'kanji', '山田「会長」はだれですか。', ['ぞくちょう', 'がくちょう', 'かいちょう', 'こくちょう'], 2, 'Đáp án C. 「会長」đọc là 「かいちょう」, nghĩa là hội trưởng/chủ tịch hội.', 12),
  q(14, 'grammar', 'マルコさんは（　　　）歌を歌うことができません。', ['すごい', '上手に', 'いろいろ', 'だけ'], 1, 'Đáp án B. 「上手に歌う」nghĩa là hát giỏi. Trạng từ 「上手に」bổ nghĩa cho động từ 「歌う」.', 13),
  q(15, 'vocabulary', '明日の（　　　）に参加しませんか。', ['イギリス', 'インタネット', 'イタリア', 'イベント'], 3, 'Đáp án D. 「イベントに参加する」nghĩa là tham gia sự kiện; các danh từ còn lại không phù hợp với 「参加する」trong câu này.', 14),
  q(16, 'grammar', '西川さんは（　　　）、イタリア語を勉強しています。', ['先生の', '学生の', '学生で', '先生'], 2, 'Đáp án C. 「学生で」là dạng て của danh từ 「学生です」, dùng để nối hai mệnh đề: là sinh viên và đang học tiếng Ý.', 15),
  q(17, 'grammar', '先週、マルコさんが靴下を（　　　）。', ['くれませんか', 'もらいました', 'もらいませんか', 'くれました'], 1, 'Đáp án B. Chủ thể Marco nhận đôi tất nên dùng 「もらいました」. 「くれました」dùng khi người khác cho người nói hoặc người thuộc nhóm người nói.', 16),
  q(18, 'grammar', 'まだ薬を（　　　）。', ['飲みました', '飲んでいません', '飲みます', '飲む'], 1, 'Đáp án B. 「まだ～ていません」diễn tả việc đến hiện tại vẫn chưa làm. Vì vậy dùng 「飲んでいません」.', 17),
  q(19, 'grammar', 'いつ大学（　）卒業しますか。', ['に', 'を', 'と', 'で'], 1, 'Đáp án B. Động từ 「卒業する」dùng trợ từ 「を」với nơi hoặc trường được tốt nghiệp: 「大学を卒業する」.', 18),
  q(20, 'grammar', '電話（　）バスの外（　）してください。', ['は・で', 'を・に', 'の・に', 'に・で'], 1, 'Đáp án B. Cấu trúc trong đề dùng 「電話を～にしてください」: đánh dấu đối tượng bằng 「を」 và nơi/đích bằng 「に」.', 19),
  q(21, 'vocabulary', '（　　　）がにひきいます。', ['チョコレート', 'ペット', 'ベッド', 'バレンタインデー'], 1, 'Đáp án B. 「にひき」là cách đếm động vật nhỏ, nên danh từ phù hợp là 「ペット」.', 20),
  q(22, 'kanji', '交差点を「みぎ」へ曲がってください。', ['右', '佐', '左', '石'], 0, 'Đáp án A. 「みぎ」được viết là 「右」, nghĩa là bên phải.', 21),
  q(23, 'vocabulary', '一日にたばこを（　　　）吸います。', ['さんばい', 'さんぼん', 'さんぽん', 'さんほん'], 1, 'Đáp án B. Điếu thuốc là vật dài nên dùng trợ từ đếm 「本」; 「3本」đọc là 「さんぼん」.', 22),
  q(24, 'vocabulary', '私は両親に（　　　）します。', ['クリスマスカード', '電話', '会話', '誕生日カード'], 1, 'Đáp án B. Cụm cố định 「人に電話します」nghĩa là gọi điện cho ai đó.', 23),
  q(25, 'vocabulary', 'マリヤムさんは私の（　　　）です。', ['じょうず', 'からだ', 'こうはい', 'へた'], 2, 'Đáp án C. 「後輩」đọc là 「こうはい」, nghĩa là đàn em/người học sau; phù hợp với cấu trúc 「私の～です」.', 24),
  q(26, 'kanji', '私の趣味は「音楽」です。', ['りょうり', 'すいえい', 'りょこう', 'おんがく'], 3, 'Đáp án D. 「音楽」đọc là 「おんがく」, nghĩa là âm nhạc.', 25),
  q(27, 'grammar', '私はあまり料理を（　　　）。', ['つくっています', 'つくります', 'つくることです', 'つくりません'], 3, 'Đáp án D. 「あまり～ません」dùng với thể phủ định để diễn tả không thường/không làm nhiều. Vì vậy dùng 「つくりません」.', 26),
  q(28, 'grammar', '（　　　）は簡単でしたが、（　　　）難しくなりました。', ['平日・週末', 'はじめ・だんだん', '毎日・毎週', 'はじめ・そろそろ'], 1, 'Đáp án B. 「はじめは～でしたが、だんだん～」diễn tả ban đầu đơn giản nhưng dần dần trở nên khó.', 27),
  q(29, 'kanji', '「じょうげ」という漢字を書いてください。', ['乗車', '下上', '上下', '上京'], 2, 'Đáp án C. 「じょうげ」được viết là 「上下」, nghĩa là trên dưới.', 28),
  q(30, 'vocabulary', 'ダニエルさんは（　　　）で、病院で働いています。', ['大学生', '高校生', '学生', 'いしゃ'], 3, 'Đáp án D. 「いしゃ」là bác sĩ; người làm việc tại bệnh viện trong câu này là 「医者」.', 29),
  q(31, 'kanji', '「ぼこくご」の漢字は何ですか。', ['帰国後', '母国後', '帰国語', '母国語'], 3, 'Đáp án D. 「ぼこくご」được viết là 「母国語」, nghĩa là tiếng mẹ đẻ.', 30),
  q(32, 'grammar', '昨日、あまり（　　　）ね。', ['さむくなかった', 'かんたんだ', 'かんたんだった', 'さむかった'], 0, 'Đáp án A. 「あまり～なかった」diễn tả không quá lạnh trong quá khứ; 「寒い」đổi sang quá khứ phủ định là 「寒くなかった」.', 31),
  q(33, 'vocabulary', '明日、（　　　）テストがありますから、今晩９時に寝ます。', ['たいせつな', 'たいへん', 'あぶない', 'めいわくな'], 0, 'Đáp án A. 「大切なテスト」nghĩa là bài kiểm tra quan trọng; tính từ な đứng trước danh từ với 「な」.', 32),
  q(34, 'vocabulary', '（　　）図書館で勉強しています。', ['だんだん', 'たいてい', 'それから', 'そろそろ'], 1, 'Đáp án B. 「たいてい」nghĩa là thường thường/thông thường, phù hợp với tần suất học ở thư viện.', 33),
  q(35, 'kanji', '明日、一緒に「映画」を見に行きませんか。', ['りょうり', 'えいが', 'かいがい', 'りょこう'], 1, 'Đáp án B. 「映画」đọc là 「えいが」, nghĩa là phim điện ảnh.', 34),
  q(36, 'vocabulary', 'テレビで（　　　）の試合を見ました。', ['ニュース', 'パンフレット', 'オリンピック', 'エアコン'], 2, 'Đáp án C. 「オリンピックの試合」là trận đấu Olympic; các lựa chọn khác không phải sự kiện thi đấu.', 35),
  q(37, 'kanji', '山田さんは「歌う」ことができますか。', ['つかう', 'もらう', 'さそう', 'うたう'], 3, 'Đáp án D. 「歌う」đọc là 「うたう」, nghĩa là hát. 「使う」đọc là 「つかう」, 「もらう」là nhận và 「誘う」đọc là 「さそう」, nên chỉ đáp án D đúng với cách đọc của Kanji 「歌」.', 36),
  q(38, 'conversation', 'A：ご飯、（　　　）？　B：うん、食べた。', ['食べた', '食べる', '飲む', '飲んだ'], 0, 'Đáp án A. Câu trả lời 「食べた」cho biết hành động đã ăn, nên câu hỏi tự nhiên là 「ご飯、食べた？」.', 37),
  q(39, 'grammar', 'パクさんはあたまが（　　　）、きれいなひとですね。', ['よいで', 'いくて', 'よくて', 'いいで'], 2, 'Đáp án C. Tính từ 「いい」có dạng て đặc biệt là 「よくて」.', 38),
  q(40, 'vocabulary', '（　　　）の信号を右に曲がってください。', ['二つ', '二つ目', '二匹', '一つ'], 1, 'Đáp án B. 「二つ目の信号」nghĩa là đèn tín hiệu thứ hai; 「目」dùng để đánh số thứ tự.', 39),
  q(41, 'vocabulary', '（　　　）を買ってきます。', ['おみやげ', 'ペンギン', 'パンフレット', '観覧車'], 0, 'Đáp án A. 「おみやげを買ってくる」là mua quà lưu niệm rồi mang về.', 40),
  q(42, 'kanji', '私の「いえ」はあそこです。', ['家', '傢', '豚', '隊'], 0, 'Đáp án A. 「いえ」trong nghĩa ngôi nhà được viết là 「家」.', 41),
  q(43, 'kanji', '毎日、学校で「勉強」しています。', ['ちょうしょく', 'じょうしゃ', 'しゅうし', 'べんきょう'], 3, 'Đáp án D. 「勉強」đọc là 「べんきょう」, nghĩa là học tập.', 42),
  q(44, 'grammar', '駅から３番（　）バス（　）乗って、美術館前（　）降りてください。', ['の・に・を', 'の・に・で', 'X・に・で', 'X・に・を'], 1, 'Đáp án B. Dùng 「3番のバス」, 「バスに乗る」và 「場所で降りる」nên bộ trợ từ đúng là 「の・に・で」.', 43),
];

// Các record được đọc và sửa trực tiếp từ ảnh bằng vision nội bộ. Chúng được
// giữ riêng khỏi bộ câu canonical cũ để có thể audit nguồn và dedupe lại.
let manualVisionQuestions = [];
try {
  const manualRecords = JSON.parse(await readFile(manualVisionPath, 'utf8'));
  manualVisionQuestions = manualRecords.map((item, index) => ({
    id: 1000 + index,
    type: item.passageText ? 'passage' : 'standalone',
    topic: item.topic,
    level: 'N5',
    questionText: item.questionText,
    explanation: item.explanation,
    orderIndex: 1000 + index,
    sourceOrder: item.sourceOrder ?? 999,
    sourceImage: item.sourceImage,
    passageText: item.passageText ?? null,
    passageId: null,
    options: item.options.map((option) => ({
      label: option.label,
      text: option.text,
      isCorrect: option.label === item.correctLabel,
    })),
  }));
} catch {
  manualVisionQuestions = [];
}

const questions = [...baseQuestions, ...manualVisionQuestions];

const normalize = (value) => String(value ?? '')
  .normalize('NFKC')
  .replace(/[Ａ-Ｚ]/g, (char) => String.fromCharCode(char.charCodeAt(0) - 0xfee0))
  .replace(/[「」『』（）()［］\[\]、。・]/g, '')
  .replace(/\s+/g, '')
  .toLowerCase();

const markdown = await readFile(sourceMarkdown, 'utf8');
const sourceQuestionCount = [...markdown.matchAll(/^### Câu\s+\d+/gmu)].length;
const sourceBlocks = [...markdown.matchAll(/^### Câu\s+\d+([\s\S]*?)(?=^### Câu\s+\d+|^## Bộ Đề:|$)/gmu)];
const sourceFingerprints = new Map();
for (const block of sourceBlocks) {
  const fingerprint = normalize(block[1]);
  if (!fingerprint) continue;
  sourceFingerprints.set(fingerprint, (sourceFingerprints.get(fingerprint) || 0) + 1);
}
const sourceDuplicateGroups = [...sourceFingerprints.values()].filter((count) => count > 1).length;
const fingerprints = new Map();
const duplicateGroups = [];
const canonicalQuestions = [];

for (const question of questions) {
  const optionSet = question.options.map((option) => normalize(option.text)).sort().join('|');
  const fingerprint = `${normalize(question.questionText)}::${optionSet}`;
  if (fingerprints.has(fingerprint)) {
    duplicateGroups.push({ keptId: fingerprints.get(fingerprint), removedId: question.id });
    continue;
  }
  fingerprints.set(fingerprint, question.id);
  const { sourceOrder, ...importQuestion } = question;
  canonicalQuestions.push(importQuestion);
}

const topicOrder = new Map(topics.map((topic) => [topic.code, topic.orderIndex]));
canonicalQuestions.sort((a, b) => (
  (topicOrder.get(a.topic) ?? 99) - (topicOrder.get(b.topic) ?? 99)
  || (a.sourceOrder ?? a.orderIndex) - (b.sourceOrder ?? b.orderIndex)
  || a.id - b.id
));

const passageIds = new Map();
const passages = [];
const preparedQuestions = canonicalQuestions.map((question, index) => {
  let passageId = null;
  if (question.passageText) {
    const key = normalize(question.passageText);
    if (!passageIds.has(key)) {
      passageId = passages.length + 1;
      passageIds.set(key, passageId);
      passages.push({
        id: passageId,
        title: `Đoạn văn ${passageId}`,
        content: question.passageText,
        topic: 'reading',
        level: 'N5',
      });
    } else {
      passageId = passageIds.get(key);
    }
  }
  const { sourceOrder, sourceImage, passageText, ...cleanQuestion } = question;
  return {
    ...cleanQuestion,
    id: index + 1,
    orderIndex: index + 1,
    passageId,
  };
});

const audit = {
  courseCode: 'jpd133',
  level: 'N5',
  sourceMarkdown: 'docs/JPD133/de-thi-jpd133.md',
  sourceQuestionCount,
  canonicalSource: 'docs/JPD133/JPD133_Đề/pdf_extracted.txt',
  canonicalQuestionCount: questions.length,
  importedQuestionCount: preparedQuestions.length,
  unresolvedSourceQuestionCount: Math.max(0, sourceQuestionCount - preparedQuestions.length),
  sourceDuplicateGroups,
  duplicateGroups,
  inferredAnswerQuestionIds: preparedQuestions.map((item) => item.id),
  ocrCorrectedQuestionIds: preparedQuestions.map((item) => item.id),
  categoryCounts: Object.fromEntries(topics.map((topic) => [topic.code, preparedQuestions.filter((item) => item.topic === topic.code).length])),
  notes: [
    'The generated Markdown contains OCR-corrupted variants with missing options and cannot be imported safely as-is.',
    'The PDF header declares 45 questions, but its extracted text contains one unnumbered item plus 43 numbered items (44 complete records); the missing OCR record is not fabricated.',
    'Unresolved OCR variants remain listed by source count and are intentionally not fabricated into database records.',
  ],
};

const importFile = { courseCode: 'jpd133', level: 'N5', topics, passages, questions: preparedQuestions };
await mkdir(path.dirname(importPath), { recursive: true });
await writeFile(importPath, `${JSON.stringify(importFile, null, 2)}\n`, 'utf8');
await writeFile(auditPath, `${JSON.stringify(audit, null, 2)}\n`, 'utf8');

const sections = topics.map((topic) => {
  const items = preparedQuestions.filter((item) => item.topic === topic.code);
  if (!items.length) return '';
  return [`## Category: ${topic.label} (${topic.code})`, '', ...items.map((item) => [
    `### Câu ${item.id}`,
    `**Câu hỏi:** ${item.questionText}`,
    '',
    '**Các phương án lựa chọn:**',
    ...item.options.map((option) => `- **${option.label}.** ${option.text}${option.isCorrect ? ' ✅' : ''}`),
    '',
    `**Đáp án:** ${item.options.find((option) => option.isCorrect).label}`,
    `**Giải thích:** ${item.explanation}`,
    '',
  ].join('\n'))].join('\n');
}).filter(Boolean).join('\n\n');
await writeFile(normalizedMarkdownPath, `# Bộ đề thi JPD133 đã chuẩn hóa\n\n> Canonical import: ${preparedQuestions.length} câu đã qua chuẩn hóa và dedupe; các bản OCR thiếu dữ liệu được ghi trong jpd133-exam-audit.json.\n\n${sections}\n`, 'utf8');

console.log(`jpd133 normalized: ${preparedQuestions.length} canonical questions`);
console.log(`source questions observed: ${sourceQuestionCount}`);
console.log(`unresolved OCR questions: ${audit.unresolvedSourceQuestionCount}`);
