import { readFile, writeFile } from 'fs/promises';
import path from 'path';

const filePath = path.join(process.cwd(), 'client', 'src', 'features', 'speaking', 'data', 'qaData.ts');

async function fix() {
  let content = await readFile(filePath, 'utf8');

  // 1. Fix the duplicate/broken lines around line 96
  content = content.replace(
    /("N は どcon\.",\s*\/\/ wait, let's write:\s*"N は どcon の N ですか。",\s*\/\/ wait, let's write:)|("N は どcon\.",\s*\/\/ wait, let's write:\s*"N は どcon の N ですか。",)/g,
    '"N は どcon の N ですか。",'
  );
  // Wait, let's also make sure we clean up any remaining "どcon" to "どこ"
  content = content.replace(/"N は どcon\.",\s*\/\/ wait, let's write:\s*/g, '');
  content = content.replace(/どcon/g, 'どこ');
  content = content.replace(/どkon/g, 'どこ');

  // 2. Fix the loop on line 170 (精度... wait, 精度... wait, ...)
  content = content.replace(
    /"Nghe câu hỏi và nhận ra từ khóa chính như 精度[^"]+"/,
    '"Nghe câu hỏi và nhận ra từ khóa chính như どこ, どのくらい, なんで, どんな, ありますか, どうですか."'
  );

  // 3. Fix the "もette行きます" and "も... -> もって来ます" duplicate/broken lines at the end
  content = content.replace(
    /"Hỏi và trả lời về việc mang theo vật gì di chuyển bằng も... -> もって来ます。",\s*\/\/ wait:\s*"Hỏi và trả lời về việc mang theo vật gì di chuyển bằng もって行きます và もって来ます。"/g,
    '"Hỏi và trả lời về việc mang theo vật gì di chuyển bằng もって行きます và もって來ます。"'
  );
  content = content.replace(
    /"Hỏi và trả lời về việc mang theo vật gì di chuyển bằng もette行きます\.\.\.",\s*\/\/ wait:\s*"Hỏi và trả lời về việc mang theo vật gì di chuyển bằng もette行きます\.\.\."/g,
    '"Hỏi và trả lời về việc mang theo vật gì di chuyển bằng もって行きます và もって来ます。"'
  );

  // Let's perform a general cleanup of any other wait comments
  content = content.replace(/\/\/ wait:[\s\S]*?\n/g, '\n');
  content = content.replace(/\/\/ wait, let's write:[\s\S]*?\n/g, '\n');

  // Correct Chinese character to Japanese/Vietnamese standard if needed (来 vs 來)
  content = content.replace(/もって來ます/g, 'もって来ます');

  await writeFile(filePath, content, 'utf8');
  console.log('Fixed qaData.ts successfully');
}

fix().catch(console.error);
