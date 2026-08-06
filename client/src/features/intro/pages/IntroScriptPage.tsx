import { Link, Navigate, useParams } from 'react-router-dom';
import type { ReactNode } from 'react';
import { ArrowLeft, ArrowRight, BookOpenText, CheckSquare, Keyboard, Lightbulb, PencilLine } from 'lucide-react';

const scriptConfig = {
  hiragana: {
    title: 'Hiragana',
    jp: 'ひらがな',
    description: 'Học mặt chữ mềm đầu tiên để đọc trợ từ, đuôi ngữ pháp và từ thuần Nhật.',
    sample: ['あ', 'い', 'う', 'え', 'お'],
    rows: 'A / K / S / T / N / H / M / R / Y / W',
  },
  katakana: {
    title: 'Katakana',
    jp: 'カタカナ',
    description: 'Làm quen bảng chữ góc cạnh để đọc từ mượn, tên riêng và thuật ngữ ngoại lai.',
    sample: ['ア', 'イ', 'ウ', 'エ', 'オ'],
    rows: 'A / K / S / T / N / H / M / R / Y / W',
  },
} as const;

type ScriptKey = keyof typeof scriptConfig;

const isScriptKey = (value?: string): value is ScriptKey => (
  value === 'hiragana' || value === 'katakana'
);

const CherryBranch = () => (
  <div className="pointer-events-none absolute -right-8 -top-12 z-10 h-36 w-52 opacity-80 sm:h-44 sm:w-64" aria-hidden="true">
    <div className="absolute right-0 top-12 h-1.5 w-48 -rotate-[18deg] rounded-full bg-[#8b6b5c]/45" />
    <div className="absolute right-20 top-8 h-1 w-24 rotate-[20deg] rounded-full bg-[#8b6b5c]/35" />
    {[
      'right-8 top-14',
      'right-16 top-9',
      'right-28 top-16',
      'right-36 top-6',
      'right-44 top-20',
      'right-24 top-28',
      'right-6 top-28',
    ].map((position) => (
      <span
        key={position}
        className={`absolute ${position} h-5 w-5 rotate-45 rounded-[60%_40%_60%_40%] bg-pink-200/80 shadow-sm`}
      />
    ))}
  </div>
);

const MountainArt = () => (
  <div className="pointer-events-none absolute bottom-0 right-[42%] hidden h-36 w-64 translate-x-1/2 opacity-60 md:block" aria-hidden="true">
    <div className="absolute bottom-0 left-4 h-24 w-56 bg-gradient-to-t from-blue-100/70 to-blue-50/20 [clip-path:polygon(50%_0,100%_100%,0_100%)]" />
    <div className="absolute bottom-10 left-[6.2rem] h-9 w-16 bg-white/80 [clip-path:polygon(50%_0,100%_100%,0_100%)]" />
    <div className="absolute bottom-2 left-0 h-10 w-72 rounded-[50%] bg-gradient-to-t from-blue-100/60 to-transparent" />
    <div className="absolute right-3 top-8 h-14 w-14 rounded-full bg-pink-200/55 blur-[1px]" />
  </div>
);

const BluePanelArt = () => (
  <div className="pointer-events-none absolute inset-0 overflow-hidden" aria-hidden="true">
    <div className="absolute bottom-0 left-0 h-24 w-full opacity-20 [background-image:repeating-radial-gradient(ellipse_at_center,transparent_0_13px,white_14px_16px,transparent_17px_28px)]" />
    <div className="absolute -right-6 bottom-0 h-64 w-44 opacity-20">
      <div className="absolute bottom-0 right-2 h-36 w-24 bg-white [clip-path:polygon(50%_0,64%_13%,57%_13%,72%_28%,63%_28%,82%_45%,71%_45%,92%_64%,8%_64%,29%_45%,18%_45%,37%_28%,28%_28%,43%_13%,36%_13%)]" />
      <div className="absolute bottom-0 right-20 h-20 w-20 bg-white [clip-path:polygon(50%_0,100%_45%,74%_45%,74%_100%,26%_100%,26%_45%,0_45%)]" />
    </div>
  </div>
);

export const IntroScriptPage = () => {
  const { script } = useParams<{ script: string }>();

  if (!isScriptKey(script)) {
    return <Navigate to="/intro" replace />;
  }

  const config = scriptConfig[script];

  return (
    <div className="relative mx-auto max-w-4xl space-y-4 px-4 py-3 animate-fade-in sm:px-6 lg:px-8">
      <CherryBranch />

      <Link
        to="/intro"
        className="relative z-20 inline-flex items-center gap-2 text-sm font-black text-blue-600 transition-colors hover:text-blue-800"
      >
        <ArrowLeft size={18} />
        Quay lại Nhập Môn
      </Link>

      <section className="relative overflow-hidden rounded-[22px] border border-blue-100 bg-white shadow-[0_18px_48px_rgba(40,88,150,0.14)]">
        <MountainArt />
        <div className="grid min-h-[195px] md:grid-cols-[1fr_0.95fr]">
          <div className="relative flex min-h-[195px] flex-col justify-center overflow-hidden px-5 py-6 sm:px-9">
            <p className="text-[10px] font-black uppercase tracking-[0.24em] text-blue-600">Bảng chữ</p>
            <h1 className="mt-3 text-3xl font-black uppercase tracking-normal text-[#091643] sm:text-4xl">
              {config.title}
            </h1>
            <div className="mt-2 font-jp text-5xl font-black tracking-normal text-[#08164a] drop-shadow-sm sm:text-6xl">
              {config.jp}
            </div>
            <p className="mt-4 max-w-sm text-sm font-bold leading-6 text-slate-600">
              {config.description}
            </p>
          </div>

          <div className="relative flex min-h-[195px] flex-col justify-center overflow-hidden bg-gradient-to-br from-[#2f8bff] via-[#1476f5] to-[#0756d8] px-5 py-6 text-white md:[clip-path:polygon(8%_0,100%_0,100%_100%,0_100%)] md:pl-12">
            <BluePanelArt />
            <div className="relative z-[1] flex flex-wrap gap-3">
              {config.sample.map((char) => (
                <span
                  key={char}
                  className="flex h-11 w-11 items-center justify-center rounded-full bg-white font-jp text-xl font-black text-blue-600 shadow-[0_10px_22px_rgba(0,0,0,0.12)]"
                >
                  {char}
                </span>
              ))}
            </div>
            <div className="relative z-[1] mt-6">
              <p className="text-[10px] font-black uppercase tracking-[0.28em] text-white/75">Các hàng chữ</p>
              <p className="mt-3 text-base font-black tracking-[0.14em] text-white">{config.rows}</p>
            </div>
          </div>
        </div>
      </section>

      <section className="grid grid-cols-1 gap-4 md:grid-cols-2">
        <IntroModeCard
          title="Học nhớ mẹo"
          subtitle="Xem từng chữ kèm hình ảnh liên tưởng, mẹo ghi nhớ và âm đọc."
          badge="Mnemonic"
          icon={<BookOpenText size={24} />}
          to={`/intro/${script}/mnemonic`}
          primary
        />
        <IntroModeCard
          title="Typing"
          subtitle="Luyện gõ romaji theo mặt chữ để kiểm tra phản xạ nhận diện kana."
          badge="Practice"
          icon={<Keyboard size={24} />}
          to={`/intro/${script}/typing`}
          kanaPreview={config.sample[0]}
        />
      </section>

      <section className="grid gap-4 md:grid-cols-3">
        <ShortcutCard
          icon={<Lightbulb size={26} />}
          title="Nhớ bằng hình"
          description="Mỗi chữ có một liên tưởng ngắn, dễ nhớ."
          tone="amber"
        />
        <ShortcutCard
          icon={<PencilLine size={26} />}
          title="Theo từng hàng"
          description="Đi theo cấu trúc bảng chữ Nhật cơ bản."
          tone="indigo"
        />
        <ShortcutCard
          icon={<CheckSquare size={26} />}
          title="Gõ để kiểm tra"
          description="Typing sẽ được nối bài tập ở bước sau."
          tone="emerald"
        />
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
  kanaPreview?: string;
};

const IntroModeCard = ({ title, subtitle, badge, icon, to, primary = false, kanaPreview }: IntroModeCardProps) => (
  <Link
    to={to}
    className={`group relative min-h-[172px] overflow-hidden rounded-[20px] border border-blue-100 p-5 shadow-[0_12px_30px_rgba(40,88,150,0.13)] transition-all hover:-translate-y-0.5 hover:border-border-hover hover:shadow-[0_18px_42px_rgba(40,88,150,0.17)] ${
      primary ? 'bg-[#071746] text-white' : 'bg-white text-[#08164a]'
    }`}
  >
    {primary ? (
      <div className="pointer-events-none absolute inset-0 overflow-hidden opacity-35" aria-hidden="true">
        <div className="absolute -right-8 bottom-0 h-36 w-44 rounded-t-full border-[18px] border-white/10" />
        <div className="absolute right-16 top-16 h-20 w-24 rounded-[45%] border-2 border-white/10" />
      </div>
    ) : (
      <div className="pointer-events-none absolute right-8 top-20 hidden h-28 w-36 rotate-[-20deg] rounded-[24px] bg-blue-50 shadow-[12px_14px_0_rgba(37,99,235,0.08)] md:block" aria-hidden="true" />
    )}

    <div className="relative z-[1] flex items-start justify-between gap-4">
      <div className={`flex h-11 w-11 items-center justify-center rounded-xl shadow-[0_8px_20px_rgba(40,88,150,0.14)] ${
        primary ? 'bg-white text-blue-600' : 'bg-blue-50 text-blue-600'
      }`}>
        {icon}
      </div>
      <span className={`rounded-full px-3 py-1.5 text-[10px] font-black uppercase tracking-[0.16em] ${
        primary ? 'bg-white/12 text-white/75' : 'bg-blue-50 text-blue-700'
      }`}>
        {badge}
      </span>
    </div>

    <div className="relative z-[1] mt-5 max-w-sm">
      <h2 className="text-2xl font-black tracking-normal">{title}</h2>
      <p className={`mt-3 min-h-[44px] text-sm font-bold leading-6 ${
        primary ? 'text-white/80' : 'text-slate-600'
      }`}>
        {subtitle}
      </p>
    </div>

    {kanaPreview && !primary && (
      <div className="relative z-[1] mt-3 flex items-center gap-3" aria-hidden="true">
        <div className="flex h-11 w-12 rotate-[-8deg] items-center justify-center rounded-xl border border-blue-100 bg-blue-50 font-jp text-xl font-black text-blue-500 shadow-[0_10px_22px_rgba(37,99,235,0.14)]">
          {kanaPreview}
        </div>
        <span className="h-2.5 w-2.5 rounded-full bg-pink-200" />
        <span className="h-2 w-2 rounded-full bg-pink-300" />
      </div>
    )}

    <div className={`relative z-[1] mt-5 inline-flex items-center gap-2 text-xs font-black uppercase tracking-[0.22em] ${
      primary ? 'text-blue-300' : 'text-blue-600'
    }`}>
      <span>Bắt đầu</span>
      <ArrowRight size={20} className="transition-transform group-hover:translate-x-1" />
    </div>
  </Link>
);

type ShortcutCardProps = {
  icon: ReactNode;
  title: string;
  description: string;
  tone: 'amber' | 'indigo' | 'emerald';
};

const shortcutTone = {
  amber: 'border-amber-200 bg-amber-50 text-amber-500 shadow-amber-100/80',
  indigo: 'border-indigo-200 bg-indigo-50 text-indigo-500 shadow-indigo-100/80',
  emerald: 'border-emerald-200 bg-emerald-50 text-emerald-600 shadow-emerald-100/80',
} as const;

const ShortcutCard = ({ icon, title, description, tone }: ShortcutCardProps) => (
  <div className="group flex items-center gap-4 rounded-[16px] border border-blue-100 bg-white p-4 shadow-[0_10px_26px_rgba(40,88,150,0.11)] transition-all hover:-translate-y-0.5 hover:border-border-hover hover:shadow-[0_15px_34px_rgba(40,88,150,0.15)]">
    <div className={`flex h-12 w-12 shrink-0 items-center justify-center rounded-full border shadow-md ${shortcutTone[tone]}`}>
      {icon}
    </div>
    <div className="min-w-0 flex-1">
      <h3 className="text-xs font-black uppercase tracking-[0.08em] text-[#08164a]">{title}</h3>
      <p className="mt-1.5 text-xs font-bold leading-5 text-slate-500">{description}</p>
    </div>
    <ArrowRight size={20} className="shrink-0 text-blue-400 transition-transform group-hover:translate-x-1" />
  </div>
);
