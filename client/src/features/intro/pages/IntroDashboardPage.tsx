import { Link } from 'react-router-dom';
import { ArrowRight, BookOpenText, Keyboard, Sparkles } from 'lucide-react';

const scripts = [
  {
    key: 'hiragana',
    title: 'Hiragana',
    jp: 'ひらがな',
    subtitle: 'Bảng chữ mềm dùng cho từ thuần Nhật, trợ từ và đuôi ngữ pháp.',
    sample: ['あ', 'か', 'さ', 'た', 'な'],
    topBg: 'bg-[#f5f2eb]',
  },
  {
    key: 'katakana',
    title: 'Katakana',
    jp: 'カタカナ',
    subtitle: 'Bảng chữ góc cạnh dùng cho từ ngoại lai, tên riêng và nhấn mạnh.',
    sample: ['ア', 'カ', 'サ', 'タ', 'ナ'],
    topBg: 'bg-[#e3edf2]',
  },
];

export const IntroDashboardPage = () => (
  <div className="mx-auto max-w-6xl space-y-6 px-4 py-3 animate-fade-in">
    <header className="space-y-2">
      <div className="inline-flex items-center gap-2 rounded-full border-2 border-slate-900 bg-white px-3 py-1 text-xs font-black uppercase tracking-[0.18em] text-blue-700 shadow-[3px_3px_0_#111827]">
        <Sparkles size={14} />
        Nền tảng chữ Nhật
      </div>
      <div>
        <h1 className="font-mono text-[40px] font-black uppercase leading-none tracking-tight text-text-primary">
          Nhập Môn <span className="text-blue-600">入門</span>
        </h1>
        <p className="mt-2 max-w-2xl text-sm font-bold uppercase tracking-widest text-text-secondary">
          Làm quen Hiragana và Katakana trước khi học từ vựng, ngữ pháp, Kanji.
        </p>
      </div>
    </header>

    <section className="grid max-w-3xl grid-cols-1 gap-4 md:grid-cols-2 mx-auto">
      {scripts.map((script) => (
        <Link
          key={script.key}
          to={`/intro/${script.key}`}
          className="group overflow-hidden rounded-[24px] border-2 border-slate-900 bg-white shadow-[6px_6px_0_#111827] transition-all hover:-translate-y-1 hover:shadow-[9px_9px_0_#111827]"
        >
          <div className={`${script.topBg} px-5 py-5 text-center`}>
            <p className="text-[10px] font-black uppercase tracking-[0.22em] text-slate-600">
              Bảng chữ nhập môn
            </p>
            <div className="mt-2 font-jp text-5xl font-black text-slate-950">{script.jp}</div>
            <h2 className="mt-1 text-2xl font-black uppercase tracking-tight text-slate-950">
              {script.title}
            </h2>
          </div>

          <div className="space-y-3 bg-sky-500 px-5 py-4 text-white">
            <p className="min-h-[40px] text-xs font-bold leading-relaxed text-white/90">{script.subtitle}</p>
            <div className="flex justify-center gap-2">
              {script.sample.map((char) => (
                <span
                  key={char}
                  className="flex h-9 w-9 items-center justify-center rounded-xl border-2 border-white/20 bg-white/10 font-jp text-lg font-black"
                >
                  {char}
                </span>
              ))}
            </div>
            <div className="flex items-center justify-between border-t border-white/20 pt-3 text-xs font-black uppercase tracking-[0.18em]">
              <span>Chọn lộ trình</span>
              <ArrowRight size={16} className="transition-transform group-hover:translate-x-1" />
            </div>
          </div>
        </Link>
      ))}
    </section>

    <section className="grid max-w-3xl gap-3 rounded-[24px] border-2 border-slate-900 bg-white p-3 shadow-[4px_4px_0_#111827] md:grid-cols-2 mx-auto">
      <div className="flex gap-3 rounded-2xl bg-[#fff9f2] p-3">
        <BookOpenText className="mt-1 shrink-0 text-[#ea580c]" size={20} />
        <div>
          <h3 className="text-xs font-black uppercase tracking-wider text-slate-900">Học nhớ mẹo</h3>
          <p className="mt-1 text-xs font-bold leading-relaxed text-slate-500">
            Dùng hình ảnh và liên tưởng để nhớ mặt chữ nhanh hơn.
          </p>
        </div>
      </div>
      <div className="flex gap-3 rounded-2xl bg-[#f4f9ff] p-3">
        <Keyboard className="mt-1 shrink-0 text-[#2563eb]" size={20} />
        <div>
          <h3 className="text-xs font-black uppercase tracking-wider text-slate-900">Typing</h3>
          <p className="mt-1 text-xs font-bold leading-relaxed text-slate-500">
            Gõ lại kana theo âm đọc để kiểm tra phản xạ nhận diện.
          </p>
        </div>
      </div>
    </section>
  </div>
);
