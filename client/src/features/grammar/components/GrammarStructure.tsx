type GrammarStructureProps = {
  structure?: string;
  small?: boolean;
  tone?: 'light' | 'dark';
};

const tokenPattern = /(\b(?:V stem|Verb stem|N\d*|V\d*|Ai?\d*|Aな\d*|S\d*|Noun\d*|Verb\d*|Adj\d*|Adjective\d*|Thể-TT)\b)/g;

const getTokenMeta = (part: string) => {
  if (/^(?:V stem|Verb stem)$/.test(part)) {
    return {
      className: 'bg-emerald-50 text-emerald-600 border-emerald-200 px-2',
      label: 'V stem',
    };
  }

  if (/^(?:N\d*|Noun\d*)$/.test(part)) {
    return {
      className: 'bg-blue-50 text-blue-600 border-blue-200',
      label: part.startsWith('Noun') ? `N${part.slice(4)}` : part,
    };
  }

  if (/^(?:V\d*|Verb\d*)$/.test(part)) {
    return {
      className: 'bg-emerald-50 text-emerald-600 border-emerald-200',
      label: part.startsWith('Verb') ? `V${part.slice(4)}` : part,
    };
  }

  if (/^(?:A\d*|Adj\d*|Adjective\d*)$/.test(part)) {
    return {
      className: 'bg-orange-50 text-orange-600 border-orange-200',
      label: part.startsWith('Adjective')
        ? `A${part.slice(9)}`
        : part.startsWith('Adj')
          ? `A${part.slice(3)}`
          : part,
    };
  }

  if (/^Ai?\d*$/.test(part)) {
    return {
      className: 'bg-orange-50 text-orange-600 border-orange-200',
      label: `Aい${part.slice(2)}`,
    };
  }

  if (/^Aな\d*$/.test(part)) {
    return {
      className: 'bg-orange-50 text-orange-600 border-orange-200',
      label: `Aな${part.slice(2)}`,
    };
  }

  if (/^S\d*$/.test(part)) {
    return {
      className: 'bg-violet-50 text-violet-600 border-violet-200',
      label: part,
    };
  }

  if (part === 'Thể-TT') {
    return {
      className: 'bg-emerald-50 text-emerald-700 border-emerald-200 px-2',
      label: part,
    };
  }

  return null;
};

export const GrammarStructure = ({ structure, small = false, tone = 'light' }: GrammarStructureProps) => {
  if (!structure) {
    return null;
  }

  return (
    <div className={`flex flex-wrap items-center gap-1.5 font-jp ${small ? 'text-sm' : ''}`}>
      {structure.split(tokenPattern).map((part, index) => {
        if (!part) {
          return null;
        }

        const meta = getTokenMeta(part);
        if (!meta) {
          return (
            <span
              key={`${part}-${index}`}
              className={
                small
                  ? `text-sm font-bold ${tone === 'dark' ? 'text-slate-300' : 'text-text-secondary'}`
                  : `text-2xl font-black ${tone === 'dark' ? 'text-white' : 'text-text-primary'}`
              }
            >
              {part}
            </span>
          );
        }

        return (
          <span
            key={`${part}-${index}`}
            className={`inline-flex min-w-[24px] items-center justify-center rounded-md border font-black shadow-sm ${
              small ? 'h-5 px-1 text-[10px]' : 'h-6 px-1.5 text-xs'
            } ${meta.className}`}
          >
            {meta.label}
          </span>
        );
      })}
    </div>
  );
};
