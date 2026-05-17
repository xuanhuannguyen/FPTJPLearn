import { Link, Navigate, useParams } from 'react-router-dom';
import type { ReactNode } from 'react';
import { ArrowLeft, ArrowRight, BookOpenText, Keyboard, Lightbulb, PencilLine } from 'lucide-react';

const scriptConfig = {
  hiragana: {
    title: 'Hiragana',
    jp: 'ひらがな',
    description: 'Học mặt chữ mềm đầu tiên để đọc trợ từ, đuôi ngữ pháp và từ thuần Nhật.',
    sample: ['あ', 'い', 'う', 'え', 'お'],
    tone: 'bg-[#f5f2eb]',
    rows: 'A / K / S / T / N / H / M / R / Y / W',
  },
  katakana: {
    title: 'Katakana',
    jp: 'カタカナ',
    description: 'Làm quen bảng chữ góc cạnh để đọc từ mượn, tên riêng và thuật ngữ ngoại lai.',
    sample: ['ア', 'イ', 'ウ', 'エ', 'オ'],
    tone: 'bg-[#e3edf2]',
    rows: 'A / K / S / T / N / H / M / R / Y / W',
  },
} as const;

type ScriptKey = keyof typeof scriptConfig;

const isScriptKey = (value?: string): value is ScriptKey => (
  value === 'hiragana' || value === 'katakana'
);

export const IntroScriptPage = () => {
  const { script } = useParams<{ script: string }>();

  if (!isScriptKey(script)) {
    return <Navigate to="/intro" replace />;
  }

  const config = scriptConfig[script];

  return (
    <div className="mx-auto max-w-3xl space-y-5 px-4 py-3 animate-fade-in">
      <header className="space-y-3">
        <Link
          to="/intro"
          className="inline-flex items-center gap-2 text-xs font-black text-blue-700 transition-colors hover:text-blue-900"
        >
          <ArrowLeft size={15} />
          Quay lại Nhập Môn
        </Link>

        <div className="grid overflow-hidden rounded-[24px] border-2 border-slate-900 bg-white shadow-[5px_5px_0_#111827] md:grid-cols-[1.1fr_0.9fr]">
          <div className={`${config.tone} flex flex-col justify-center p-5`}>
            <p className="text-[9px] font-black uppercase tracking-[0.22em] text-slate-600">Bảng chữ</p>
            <h1 className="mt-1 text-3xl font-black uppercase tracking-tight text-slate-950">{config.title}</h1>
            <div className="mt-2 font-jp text-5xl font-black text-slate-950">{config.jp}</div>
            <p className="mt-3 max-w-xl text-xs font-bold leading-relaxed text-slate-700">{config.description}</p>
          </div>
          <div className="flex flex-col justify-center gap-3 bg-sky-500 p-5 text-white">
            <div className="flex gap-2">
              {config.sample.map((char) => (
                <span
                  key={char}
                  className="flex h-10 w-10 items-center justify-center rounded-xl border-2 border-white/20 bg-white/10 font-jp text-xl font-black"
                >
                  {char}
                </span>
              ))}
            </div>
            <div>
              <p className="text-[9px] font-black uppercase tracking-[0.22em] text-white/60">Các hàng chữ</p>
              <p className="mt-0.5 text-xs font-black tracking-wide">{config.rows}</p>
            </div>
          </div>
        </div>
      </header>

      <section className="grid grid-cols-1 gap-4 md:grid-cols-2">
        <IntroModeCard
          title="Học nhớ mẹo"
          subtitle="Xem từng chữ kèm hình ảnh liên tưởng, mẹo ghi nhớ và âm đọc."
          badge="Mnemonic"
          icon={<BookOpenText size={20} />}
          to={`/intro/${script}/mnemonic`}
          primary
        />
        <IntroModeCard
          title="Typing"
          subtitle="Luyện gõ romaji theo mặt chữ để kiểm tra phản xạ nhận diện kana."
          badge="Practice"
          icon={<Keyboard size={20} />}
          to={`/intro/${script}/typing`}
        />
      </section>

      <section className="grid gap-3 md:grid-cols-3">
        <div className="rounded-xl border-2 border-slate-200 bg-white p-3">
          <Lightbulb size={18} className="text-amber-500" />
          <h3 className="mt-2 text-xs font-black uppercase tracking-wider text-slate-900">Nhớ bằng hình</h3>
          <p className="mt-1 text-xs font-bold leading-normal text-slate-500">Mỗi chữ có một liên tưởng ngắn, dễ nhớ.</p>
        </div>
        <div className="rounded-xl border-2 border-slate-200 bg-white p-3">
          <PencilLine size={18} className="text-blue-600" />
          <h3 className="mt-2 text-xs font-black uppercase tracking-wider text-slate-900">Theo từng hàng</h3>
          <p className="mt-1 text-xs font-bold leading-normal text-slate-500">Đi theo cấu trúc bảng chữ Nhật cơ bản.</p>
        </div>
        <div className="rounded-xl border-2 border-slate-200 bg-white p-3">
          <Keyboard size={18} className="text-emerald-600" />
          <h3 className="mt-2 text-xs font-black uppercase tracking-wider text-slate-900">Gõ để kiểm tra</h3>
          <p className="mt-1 text-xs font-bold leading-normal text-slate-500">Typing sẽ được nối bài tập ở bước sau.</p>
        </div>
      </section>
    </div>
  );
};

type IntroModeCardProps = {
  title: string;
  subtitle: string;
  badge: string;
  icon: ReactNode;
  to: string;
  primary?: boolean;
};

const IntroModeCard = ({ title, subtitle, badge, icon, to, primary = false }: IntroModeCardProps) => (
  <Link
    to={to}
    className={`group rounded-[20px] border-2 border-slate-900 p-4 shadow-[4px_4px_0_#111827] transition-all hover:-translate-y-1 hover:shadow-[6px_6px_0_#111827] ${
      primary ? 'bg-slate-950 text-white' : 'bg-white text-slate-950'
    }`}
  >
    <div className="flex items-start justify-between gap-4">
      <div className={`flex h-10 w-10 items-center justify-center rounded-xl ${
        primary ? 'bg-white text-blue-700' : 'bg-blue-50 text-blue-700'
      }`}>
        {icon}
      </div>
      <span className={`rounded-full px-2.5 py-0.5 text-[9px] font-black uppercase tracking-[0.18em] ${
        primary ? 'bg-white/10 text-white/70' : 'bg-slate-100 text-slate-500'
      }`}>
        {badge}
      </span>
    </div>
    <h2 className="mt-4 text-[22px] font-black tracking-tight">{title}</h2>
    <p className={`mt-1.5 min-h-[36px] text-xs font-bold leading-normal ${
      primary ? 'text-white/70' : 'text-slate-500'
    }`}>
      {subtitle}
    </p>
    <div className="mt-4 flex items-center gap-2 text-[10px] font-black uppercase tracking-[0.18em]">
      <span>Bắt đầu</span>
      <ArrowRight size={15} className="transition-transform group-hover:translate-x-1" />
    </div>
  </Link>
);
