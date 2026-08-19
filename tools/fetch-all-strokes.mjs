import fs from 'node:fs';
import path from 'node:path';

const root = process.cwd();

async function downloadMissing() {
  const characters = new Set();
  const dirs = ['jpd113', 'jpd123', 'jpd133'];
  for (const dir of dirs) {
    const lessonDir = path.join(root, 'client', 'public', 'data', 'kanji', dir, 'lessons');
    if (!fs.existsSync(lessonDir)) continue;
    const files = fs.readdirSync(lessonDir).filter((f) => f.endsWith('.json'));
    for (const f of files) {
      const data = JSON.parse(fs.readFileSync(path.join(lessonDir, f), 'utf8'));
      for (const k of data.kanjiItems || []) {
        characters.add(k.character);
      }
    }
  }

  const outDir = path.join(root, 'client', 'public', 'data', 'kanji', 'strokes-jp');
  fs.mkdirSync(outDir, { recursive: true });
  const all = [...characters];
  let downloaded = 0;
  const failed = [];

  for (const char of all) {
    const filePath = path.join(outDir, `${char}.json`);
    if (fs.existsSync(filePath)) continue;

    let data = null;
    // Try Japanese stroke data first
    try {
      const res = await fetch(`https://cdn.jsdelivr.net/npm/hanzi-writer-data-jp@0/${encodeURIComponent(char)}.json`);
      if (res.ok) data = await res.json();
    } catch (e) {
      console.warn(`JP fetch failed for ${char}:`, e.message);
    }

    // Fallback to standard hanzi-writer data
    if (!data) {
      try {
        const res = await fetch(`https://cdn.jsdelivr.net/npm/hanzi-writer-data@2.0/${encodeURIComponent(char)}.json`);
        if (res.ok) data = await res.json();
      } catch (e) {
        console.warn(`Standard fetch failed for ${char}:`, e.message);
      }
    }

    if (data) {
      fs.writeFileSync(
        filePath,
        JSON.stringify(
          {
            character: char,
            strokes: data.strokes || [],
            medians: data.medians || [],
            strokeCount: (data.strokes || []).length,
            source: 'hanzi-writer-data',
          },
          null,
          2,
        ),
      );
      downloaded++;
      console.log(`Downloaded ${char} (${downloaded})`);
    } else {
      failed.push(char);
      console.error(`Failed to download ${char}`);
    }
  }

  console.log(`\nDone! Downloaded: ${downloaded}, Failed: ${failed.length}`);
  if (failed.length > 0) console.log('Failed characters:', failed);
}

await downloadMissing();
