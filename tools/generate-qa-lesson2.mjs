import { readFile, writeFile, copyFile, mkdir } from 'node:fs/promises';
import path from 'node:path';

const root = process.cwd();
const materialDir = path.join(root, 'material', 'Luyện Nói', 'Vấn đáp JPTJPLearn', 'JPD113', 'Bài 2');
const targetDir = path.join(root, 'client', 'public', 'data', 'speaking', 'jpd113', 'qa');

async function run() {
  await mkdir(targetDir, { recursive: true });

  // 1. Process CauHoiKhongTranh.md
  console.log('Processing CauHoiKhongTranh.md...');
  const noImgContent = await readFile(path.join(materialDir, 'CauHoiKhongTranh.md'), 'utf8');
  const noImgJsonStr = noImgContent.match(/```json\r?\n([\s\S]+?)\r?\n```/)[1];
  const noImgData = JSON.parse(noImgJsonStr);
  await writeFile(
    path.join(targetDir, 'lesson2_no_image.json'),
    JSON.stringify(noImgData, null, 2) + '\n',
    'utf8'
  );
  console.log('Wrote lesson2_no_image.json');

  // 2. Process Cauhoicotranh.md
  console.log('Processing Cauhoicotranh.md...');
  const withImgContent = await readFile(path.join(materialDir, 'Cauhoicotranh.md'), 'utf8');
  const withImgJsonStr = withImgContent.match(/```json\r?\n([\s\S]+?)\r?\n```/)[1];
  const withImgData = JSON.parse(withImgJsonStr);

  // Map image URLs
  const imageMap = {
    'jpd113_l2_picture_01_shopping_building': '/data/speaking/jpd113/qa/hinh1.png',
    'jpd113_l2_picture_02_bag_product': '/data/speaking/jpd113/qa/hinh2.png',
    'jpd113_l2_picture_03_iphone_product': '/data/speaking/jpd113/qa/hinh3.png',
    'jpd113_l2_picture_04_cafe_order': '/data/speaking/jpd113/qa/hinh4.png'
  };

  for (const set of withImgData.pictureSets) {
    if (imageMap[set.pictureId]) {
      set.imageUrl = imageMap[set.pictureId];
    } else {
      console.warn(`Warning: No image mapped for pictureId: ${set.pictureId}`);
    }
  }

  await writeFile(
    path.join(targetDir, 'lesson2_with_image.json'),
    JSON.stringify(withImgData, null, 2) + '\n',
    'utf8'
  );
  console.log('Wrote lesson2_with_image.json');

  // 3. Copy image files
  console.log('Copying images...');
  const images = ['hinh1.png', 'hinh2.png', 'hinh3.png', 'hinh4.png'];
  for (const img of images) {
    await copyFile(
      path.join(materialDir, img),
      path.join(targetDir, img)
    );
    console.log(`Copied ${img}`);
  }

  console.log('All operations completed successfully!');
}

run().catch(console.error);
