import { Link } from 'react-router-dom';
import { ArrowRight, BookOpenText, Keyboard, NotebookPen, Sparkles } from 'lucide-react';

const scripts = [
  {
    key: 'hiragana',
    title: 'Hiragana',
    jp: 'ひらがな',
    subtitle: 'Bảng chữ mềm dùng cho từ thuần Nhật, trợ từ và đuôi ngữ pháp.',
    sample: ['あ', 'か', 'さ', 'た', 'な'],
    accent: 'text-blue-600',
    bar: 'from-sky-400 to-blue-600',
    chip: 'bg-blue-50 text-blue-700',
    icon: <BookOpenText size={24} />,
  },
  {
    key: 'katakana',
    title: 'Katakana',
    jp: 'カタカナ',
    subtitle: 'Bảng chữ góc cạnh dùng cho từ ngoại lai, tên riêng và nhấn mạnh.',
    sample: ['ア', 'カ', 'サ', 'タ', 'ナ'],
    accent: 'text-sky-600',
    bar: 'from-sky-400 to-blue-600',
    chip: 'bg-sky-50 text-sky-700',
    icon: <NotebookPen size={24} />,
  },
];

const HeroArt = () => (
  <div className="pointer-events-none absolute inset-0 overflow-hidden rounded-[28px]" aria-hidden="true">
    {/* Nền gradient fallback luôn chạy phía dưới */}
    <div className="absolute inset-0 bg-gradient-to-br from-white via-[#eef6ff] to-[#dbeafe]" />

    {/* Mặt trời Nhật Bản */}
    <div className="absolute right-8 top-6 h-14 w-14 rounded-full bg-gradient-to-br from-rose-400 to-rose-500 opacity-90 shadow-[0_0_44px_rgba(244,63,94,0.35)]" />

    {/* Núi Phú Sĩ */}
    <div className="absolute bottom-0 right-24 hidden h-36 w-52 translate-x-1/4 md:block">
      <div className="absolute bottom-0 left-2 h-32 w-44 bg-gradient-to-t from-[#7ba4c9] to-[#d5e7f6] [clip-path:polygon(50%_0,100%_100%,0_100%)]" />
      <div className="absolute bottom-20 left-11 h-9 w-14 bg-white/85 [clip-path:polygon(50%_0,100%_100%,0_100%)]" />
    </div>

    {/* Chùa */}
    <div className="absolute bottom-0 right-[3rem] hidden h-28 w-20 md:block">
      <div className="absolute bottom-0 left-1/2 h-20 w-12 -translate-x-1/2 bg-[#0b2f5c]/70 [clip-path:polygon(50%_0,100%_100%,0_100%)]" />
      <div className="absolute bottom-16 left-1/2 h-5 w-14 -translate-x-1/2 rounded-sm bg-[#0b2f5c]/70" />
      <div className="absolute bottom-[4.5rem] left-1/2 h-3 w-3 -translate-x-1/2 rounded-full bg-[#0b2f5c]/80" />
      <div className="absolute bottom-5 left-1/2 h-8 w-6 -translate-x-1/2 rounded-t-[10px] bg-[#0b2f5c]/70" />
    </div>

    {/* Hoa anh đào */}
    <div className="absolute right-14 top-1/2 h-16 w-16 rotate-45 rounded-[60%_40%_60%_40%] bg-pink-200/70 shadow-sm" />
    <div className="absolute right-36 top-16 h-6 w-6 rotate-45 rounded-[60%_40%_60%_40%] bg-pink-200/80 shadow-sm" />
    <div className="absolute right-24 bottom-24 h-5 w-5 rotate-45 rounded-[60%_40%_60%_40%] bg-pink-200/70 shadow-sm" />
    <div className="absolute right-40 top-24 h-4 w-4 rotate-45 rounded-[60%_40%_60%_40%] bg-pink-200/70 shadow-sm" />
  </div>
);

export const IntroDashboardPage = () => (
  <div className="mx-auto max-w-6xl space-y-6 px-4 py-4 animate-fade-in md:px-6 lg:px-8">
    {/* Hero */}
    <section className="relative overflow-hidden rounded-[28px] border border-blue-100 bg-white px-5 py-8 shadow-[0_18px_50px_rgba(40,88,150,0.14)] md:px-9 md:py-10">
      <HeroArt />

      <div className="relative z-[1] max-w-xl">
        <div className="inline-flex items-center gap-2 rounded-full border border-blue-100 bg-white/90 px-3.5 py-1.5 text-[10px] font-black uppercase tracking-[0.22em] text-blue-700 shadow-sm backdrop-blur-sm">
          <Sparkles size={13} />
          Nền tảng chữ Nhật
        </div>

        <h1 className="mt-4 font-heading text-4xl font-black uppercase leading-[0.95] tracking-tight text-[#08164a] sm:text-5xl">
          Nhập Môn <span className="text-blue-600">入門</span>
        </h1>
        <p className="mt-4 max-w-md text-sm font-bold leading-6 text-slate-500">
          Làm quen Hiragana và Katakana trước khi học từ vựng, ngữ pháp, Kanji. Bắt đầu từ nền tảng vững chắc.
        </p>
      </div>
    </section>

    {/* Hai thẻ Hiragana / Katakana */}
    <section className="grid grid-cols-1 gap-5 lg:grid-cols-2">
      {scripts.map((script) => (
        <Link
          key={script.key}
          to={`/intro/${script.key}`}
          className="group relative flex flex-col overflow-hidden rounded-[26px] border border-blue-100 bg-white p-6 text-left shadow-[0_16px_44px_rgba(40,88,150,0.11)] transition-all hover:-translate-y-1 hover:border-blue-200 hover:shadow-[0_22px_58px_rgba(40,88,150,0.17)] md:p-7"
        >
          {/* Viền trái xanh */}
          <div className={`absolute left-0 top-6 h-[calc(100%-3rem)] w-1.5 rounded-r-full bg-gradient-to-b ${script.bar}`} />

          <div className="flex items-start justify-between gap-4">
            <div className="flex h-14 w-14 shrink-0 items-center justify-center rounded-2xl border border-blue-100 bg-blue-50 text-blue-600 shadow-[0_10px_22px_rgba(37,99,235,0.14)]">
              {script.icon}
            </div>
            <span className={`inline-flex h-7 items-center rounded-lg px-3 text-[10px] font-black uppercase tracking-[0.16em] ${script.chip}`}>
              {script.key}
            </span>
          </div>

          <div className="mt-5">
            <p className="text-[10px] font-black uppercase tracking-[0.24em] text-slate-400">
              Bảng chữ nhập môn
            </p>
            <div className="mt-2 flex flex-wrap items-end gap-x-4 gap-y-1">
              <div className={`font-jp text-5xl font-black leading-none ${script.accent}`}>{script.jp}</div>
              <h2 className="text-2xl font-black uppercase tracking-tight text-[#08164a]">
                {script.title}
              </h2>
            </div>
          </div>

          <p className="mt-4 min-h-[40px] text-sm font-bold leading-6 text-slate-500">
            {script.subtitle}
          </p>

          <div className="mt-5 flex flex-wrap gap-2.5">
            {script.sample.map((char) => (
              <span
                key={char}
                className="flex h-10 w-10 items-center justify-center rounded-full border border-blue-100 bg-slate-50 font-jp text-lg font-black text-[#08164a] shadow-sm transition-all group-hover:border-blue-200 group-hover:bg-white"
              >
                {char}
              </span>
            ))}
          </div>

          <div className="mt-6 inline-flex items-center gap-2 self-start rounded-xl bg-gradient-to-r from-blue-500 to-blue-700 px-5 py-3 text-xs font-black uppercase tracking-[0.18em] text-white shadow-[0_10px_22px_rgba(37,99,235,0.28)] transition-all group-hover:-translate-y-0.5 group-hover:shadow-[0_14px_28px_rgba(37,99,235,0.36)]">
            <span>Chọn lộ trình</span>
            <ArrowRight size={16} className="transition-transform group-hover:translate-x-1" />
          </div>
        </Link>
      ))}
    </section>

    {/* Giới thiệu tính năng */}
    <section className="grid grid-cols-1 gap-5 rounded-[26px] border border-blue-100 bg-white p-5 shadow-[0_16px_44px_rgba(40,88,150,0.11)] md:grid-cols-2 md:p-6">
      <div className="flex items-center gap-4 rounded-[20px] border border-orange-100 bg-gradient-to-br from-orange-50 to-amber-50/60 p-5">
        <div className="flex h-14 w-14 shrink-0 items-center justify-center rounded-2xl bg-orange-100 text-orange-600 shadow-[0_10px_22px_rgba(234,88,12,0.16)]">
          <BookOpenText size={26} />
        </div>
        <div className="min-w-0">
          <h3 className="text-sm font-black uppercase tracking-[0.1em] text-orange-700">
            Học nhớ mẹo
          </h3>
          <p className="mt-1 text-sm font-bold leading-6 text-slate-500">
            Dùng hình ảnh và liên tưởng để nhớ mặt chữ nhanh hơn.
          </p>
        </div>
      </div>

      <div className="flex items-center gap-4 rounded-[20px] border border-sky-100 bg-gradient-to-br from-sky-50 to-blue-50/60 p-5">
        <div className="flex h-14 w-14 shrink-0 items-center justify-center rounded-2xl bg-sky-100 text-sky-600 shadow-[0_10px_22px_rgba(14,165,233,0.16)]">
          <Keyboard size={26} />
        </div>
        <div className="min-w-0">
          <h3 className="text-sm font-black uppercase tracking-[0.1em] text-sky-700">
            Typing
          </h3>
          <p className="mt-1 text-sm font-bold leading-6 text-slate-500">
            Gõ lại kana theo âm đọc để kiểm tra phản xạ nhận diện.
          </p>
        </div>
      </div>
    </section>
  </div>
);
