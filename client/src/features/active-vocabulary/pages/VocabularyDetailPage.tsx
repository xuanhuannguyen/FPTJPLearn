import { useEffect, useState } from 'react';
import { useParams, useNavigate, useSearchParams } from 'react-router-dom';
import { ArrowLeft, Trash2, Plus, Play, Loader2, BookOpen, Search } from 'lucide-react';
import { vocabularyApi } from '../api/vocabularyApi';
import { AddWordModal } from '../components/AddWordModal';
import { ConfirmModal } from '../../../shared/components/ConfirmModal';
import { ReviewWorkspace } from '../../review/components/ReviewWorkspace';

const levelBadgeStyles = [
  'bg-slate-200 text-slate-700',
  'bg-amber-100 text-amber-700',
  'bg-orange-100 text-orange-700',
  'bg-emerald-100 text-emerald-700',
];

const getProgressLevel = (level: number) => Math.min(Math.max(level, 0), 3);
const getProgressLabel = (level: number) => getProgressLevel(level) >= 3 ? 'Mastered' : `Level ${getProgressLevel(level)}`;

type VocabularyItem = {
  id: string;
  word: string;
  reading: string;
  wordType: string;
  meaning: string;
  exampleSentence?: string;
  exampleMeaning?: string;
  orderIndex: number;
  level: number;
  status: string;
};

type VocabularyListDetail = {
  id: string;
  name: string;
  description?: string;
  wordCount: number;
  createdAt: string;
  items: VocabularyItem[];
};

export const VocabularyDetailPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [searchParams, setSearchParams] = useSearchParams();
  const [list, setList] = useState<VocabularyListDetail | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [isDeleting, setIsDeleting] = useState(false);
  const [deletingItemId, setDeletingItemId] = useState<string | null>(null);
  const [deleteError, setDeleteError] = useState('');
  const [isAddWordOpen, setIsAddWordOpen] = useState(false);
  const [isReviewWorkspaceOpen, setIsReviewWorkspaceOpen] = useState(searchParams.get('study') === '1');
  const [wordSearchQuery, setWordSearchQuery] = useState('');
  
  // Confirmation states
  const [deleteListConfirm, setDeleteListConfirm] = useState(false);
  const [deleteItemConfirm, setDeleteItemConfirm] = useState<string | null>(null);

  useEffect(() => {
    let cancelled = false;

    const loadDetail = async () => {
      try {
        if (!id) return;
        setIsLoading(true);
        const data = await vocabularyApi.getListById(id);
        if (!cancelled) {
          setList(data);
        }
      } catch (err) {
        console.error(err);
        if (!cancelled) {
          navigate('/active-vocabulary');
        }
      } finally {
        if (!cancelled) {
          setIsLoading(false);
        }
      }
    };

    void loadDetail();

    return () => {
      cancelled = true;
    };
  }, [id, navigate]);

  const fetchDetail = async () => {
    if (!id) return;
    const data = await vocabularyApi.getListById(id);
    setList(data);
  };

  const executeDeleteList = async () => {
    try {
      setIsDeleting(true);
      setDeleteError('');
      await vocabularyApi.deleteList(id!);
      navigate('/active-vocabulary');
    } catch (err) {
      console.error('Failed to delete list', err);
      setDeleteError('Could not delete this vocabulary list. Please try again.');
      setIsDeleting(false);
      throw err;
    }
  };

  const executeDeleteItem = async (itemId: string) => {
    try {
      setDeletingItemId(itemId);
      setDeleteError('');
      await vocabularyApi.deleteItem(itemId);
      setList((prev) => prev ? ({
        ...prev,
        items: prev.items.filter((item) => item.id !== itemId),
        wordCount: Math.max(0, prev.wordCount - 1)
      }) : prev);
    } catch (err) {
      console.error('Failed to delete item', err);
      setDeleteError('Could not remove this word. Please try again.');
      throw err;
    } finally {
      setDeletingItemId(null);
    }
  };

  if (isLoading) {
    return (
      <div className="flex flex-col items-center justify-center h-[60vh] text-text-secondary">
        <Loader2 size={40} className="animate-spin mb-4 text-accent-primary" />
        <p>Loading vocabulary list...</p>
      </div>
    );
  }

  if (!list) return null;

  const filteredItems = list.items.filter((item) => {
    const normalizedQuery = wordSearchQuery.trim().toLowerCase();
    if (!normalizedQuery) {
      return true;
    }

    const haystack = [
      item.word,
      item.reading,
      item.meaning,
      item.wordType,
      item.exampleSentence,
      item.exampleMeaning,
    ]
      .filter(Boolean)
      .join(' ')
      .toLowerCase();

    return haystack.includes(normalizedQuery);
  });

  return (
    <div className="space-y-8 animate-fade-in pb-20">
      {!isReviewWorkspaceOpen && (
        <>
          {/* Header section */}
          <div className="mb-4 flex items-center gap-4 text-text-secondary">
            <button 
              onClick={() => navigate('/active-vocabulary')}
              className="btn-secondary"
            >
              <ArrowLeft size={20} />
              Quay lại
            </button>
          </div>

          <div className="app-surface relative overflow-hidden rounded-[32px] p-6 md:p-8">
            <div className="pointer-events-none absolute right-0 top-0 h-72 w-72 rounded-full bg-accent-primary/10 blur-3xl" />
            <div className="pointer-events-none absolute bottom-0 left-20 h-40 w-40 rounded-full bg-accent-cta/10 blur-3xl" />

            <div className="relative flex flex-col justify-between gap-6 md:flex-row md:items-end">
              <div>
                <p className="mb-2 text-xs font-black uppercase tracking-[0.22em] text-accent-primary">Active Vocabulary Set</p>
                <h1 className="mb-3 text-4xl font-black tracking-tight text-text-primary">{list.name}</h1>
                <p className="max-w-3xl text-lg leading-8 text-text-secondary">{list.description}</p>
                <div className="mt-6 flex flex-wrap items-center gap-3">
                  <div className="rounded-2xl bg-bg-tertiary px-4 py-2 text-sm font-bold text-text-primary">
                    {list.wordCount} words
                  </div>
                  <div className="rounded-2xl bg-accent-primary/10 px-4 py-2 text-sm font-bold text-accent-primary">
                    Created {new Date(list.createdAt).toLocaleDateString()}
                  </div>
                </div>
              </div>

              <div className="flex items-center gap-3">
                <button 
                  onClick={() => {
                    setDeleteError('');
                    setDeleteListConfirm(true);
                  }}
                  disabled={isDeleting}
                  className="flex h-12 w-12 items-center justify-center rounded-2xl border border-accent-danger/20 bg-accent-danger/10 text-accent-danger transition-colors hover:bg-accent-danger/15 disabled:opacity-50"
                  title="Delete List"
                >
                  <Trash2 size={20} />
                </button>
                
                <button 
                  onClick={() => {
                    setIsReviewWorkspaceOpen(true);
                    setSearchParams({ study: '1' });
                  }}
                  className="btn-primary"
                >
                  <Play size={20} className="fill-current" />
                  <span>Học ngay</span>
                </button>
              </div>
            </div>
          </div>
        </>
      )}

      {isReviewWorkspaceOpen ? (
        <ReviewWorkspace
          listId={id!}
          onClose={async () => {
            setIsReviewWorkspaceOpen(false);
            setSearchParams({});
            await fetchDetail();
          }}
        />
      ) : (
        <>
          {/* Action Bar */}
          <div className="app-surface flex flex-col gap-4 rounded-[28px] p-4 lg:flex-row lg:items-center lg:justify-between">
            <div className="flex flex-col gap-3 md:flex-row md:items-center">
              <h2 className="text-xl font-bold text-text-primary flex items-center gap-2">
                <BookOpen size={24} className="text-accent-primary" />
                Danh sách từ vựng
              </h2>
              <div className="relative w-full md:w-80">
                <label htmlFor="word-search" className="sr-only">
                  Search words in this list
                </label>
                <Search className="absolute left-3 top-1/2 -translate-y-1/2 text-text-muted" size={18} />
                <input
                  id="word-search"
                  type="text"
                  value={wordSearchQuery}
                  onChange={(event) => setWordSearchQuery(event.target.value)}
                  placeholder="Tìm kiếm từ trong bộ này..."
                  className="h-11 w-full rounded-xl border-2 border-border bg-white px-10 text-sm font-bold shadow-sm outline-none transition-all focus:border-accent-primary"
                />
              </div>
            </div>
            <button 
              onClick={() => setIsAddWordOpen(true)}
              className="btn-secondary"
            >
              <Plus size={20} />
              Thêm từ
            </button>
          </div>

          {/* Words List */}
          <div className="overflow-hidden rounded-[28px] border-2 border-border bg-bg-secondary shadow-card">
            <div className="hidden grid-cols-[56px_minmax(260px,1.25fr)_minmax(220px,1fr)_112px_44px] items-center gap-6 border-b-2 border-border bg-bg-tertiary/80 px-6 py-3 text-[11px] font-bold uppercase tracking-wider text-text-muted lg:grid">
              <div>No.</div>
              <div>Từ vựng</div>
              <div>Nghĩa</div>
              <div className="text-center">Tiến độ</div>
              <div aria-hidden="true" />
            </div>
            <div className="divide-y divide-border">
              {filteredItems.map((item, index) => (
                <div
                  key={item.id}
                  className="group grid gap-4 px-4 py-5 transition-colors hover:bg-bg-tertiary/50 md:px-6 lg:grid-cols-[56px_minmax(260px,1.25fr)_minmax(220px,1fr)_112px_44px] lg:items-center lg:gap-6 lg:py-6"
                >
                  <div className="hidden font-mono text-sm font-bold text-text-muted lg:block">
                    {index + 1}.
                  </div>

                  <div className="grid min-w-0 grid-cols-[36px_1fr_auto] items-start gap-3 lg:block">
                    <div className="font-mono text-sm font-bold text-text-muted lg:hidden">
                      {index + 1}.
                    </div>
                    <div className="min-w-0">
                      <div className="flex min-w-0 flex-wrap items-baseline gap-x-4 gap-y-1">
                        <span className="font-heading text-2xl font-bold leading-tight text-text-primary md:text-3xl">
                          {item.word}
                        </span>
                        <span className="font-jp text-base font-medium leading-tight text-text-secondary">
                          {item.reading}
                        </span>
                      </div>
                      <span className="mt-3 inline-flex max-w-full items-center rounded bg-bg-tertiary px-2 py-0.5 text-xs font-medium text-text-secondary">
                        {item.wordType}
                      </span>
                    </div>
                    <div className="flex flex-col items-end gap-2 lg:hidden">
                      <div
                        className={`rounded-full px-3 py-1 text-xs font-semibold ${levelBadgeStyles[getProgressLevel(item.level)] || levelBadgeStyles[0]}`}
                        title={getProgressLabel(item.level)}
                      >
                        {getProgressLabel(item.level)}
                      </div>
                    </div>
                  </div>

                  <div className="min-w-0 pl-12 lg:pl-0">
                    <p className="text-base font-semibold leading-6 text-text-primary md:text-lg">{item.meaning}</p>
                    {item.exampleSentence && (
                      <div className="mt-2 text-sm leading-6">
                        <p className="font-medium text-text-secondary">{item.exampleSentence}</p>
                        <p className="text-text-muted">{item.exampleMeaning}</p>
                      </div>
                    )}
                  </div>

                  <div className="hidden flex-col items-center gap-3 lg:flex">
                    <div
                      className={`min-w-[80px] rounded-full px-3 py-1 text-center text-xs font-semibold ${levelBadgeStyles[getProgressLevel(item.level)] || levelBadgeStyles[0]}`}
                      title={getProgressLabel(item.level)}
                    >
                      {getProgressLabel(item.level)}
                    </div>
                    <div className={`min-w-[74px] rounded px-2 py-0.5 text-center text-[10px] font-bold uppercase tracking-wider ${
                      item.status === 'new' ? 'bg-accent-info/10 text-accent-info' :
                      item.status === 'learning' || item.status === 'relearning' ? 'bg-accent-warning/10 text-accent-warning' :
                      item.status === 'review' ? 'bg-sky-100 text-sky-700' :
                      'bg-accent-success/10 text-accent-success'
                    }`}>
                      {item.status}
                    </div>
                  </div>

                  <div className="flex items-center justify-end gap-3 pl-12 lg:pl-0">
                    <div className={`rounded px-2 py-0.5 text-[10px] font-bold uppercase tracking-wider lg:hidden ${
                      item.status === 'new' ? 'bg-accent-info/10 text-accent-info' :
                      item.status === 'learning' || item.status === 'relearning' ? 'bg-accent-warning/10 text-accent-warning' :
                      item.status === 'review' ? 'bg-sky-100 text-sky-700' :
                      'bg-accent-success/10 text-accent-success'
                    }`}>
                      {item.status}
                    </div>
                    <button 
                      onClick={() => {
                        setDeleteError('');
                        setDeleteItemConfirm(item.id);
                      }}
                      disabled={deletingItemId === item.id}
                      className="flex h-9 w-9 items-center justify-center rounded-lg text-text-muted opacity-100 transition-colors hover:bg-accent-danger/10 hover:text-accent-danger focus:outline-none focus:ring-2 focus:ring-accent-danger/30 disabled:cursor-not-allowed disabled:opacity-50 lg:opacity-0 lg:group-hover:opacity-100 lg:group-focus-within:opacity-100"
                      title="Remove Word"
                    >
                      <Trash2 size={16} />
                    </button>
                  </div>
                </div>
              ))}

              {list.items.length === 0 && (
                <div className="p-12 text-center text-text-secondary">
                  This list is empty. Add some words to start learning!
                </div>
              )}

              {list.items.length > 0 && filteredItems.length === 0 && (
                <div className="p-12 text-center text-text-secondary">
                  No words match "<span className="font-medium">{wordSearchQuery}</span>".
                </div>
              )}
            </div>
          </div>
        </>
      )}

      <AddWordModal 
        isOpen={isAddWordOpen} 
        listId={id!} 
        onClose={() => setIsAddWordOpen(false)} 
        onSuccess={(newItem: VocabularyItem) => {
          setList((prev) => prev ? ({
            ...prev,
            items: [...prev.items, newItem],
            wordCount: prev.wordCount + 1
          }) : prev);
        }} 
      />

      <ConfirmModal 
        isOpen={deleteListConfirm}
        title="Delete Vocabulary List"
        message="Are you sure you want to delete this entire list? All progress and words will be permanently lost."
        confirmText="Delete List"
        onConfirm={executeDeleteList}
        onCancel={() => {
          setDeleteListConfirm(false);
          setDeleteError('');
        }}
        errorMessage={deleteError}
      />

      <ConfirmModal 
        isOpen={!!deleteItemConfirm}
        title="Remove Word"
        message="Are you sure you want to remove this word from the list?"
        confirmText="Remove"
        onConfirm={() => {
          if (deleteItemConfirm) {
            return executeDeleteItem(deleteItemConfirm);
          }
        }}
        onCancel={() => {
          setDeleteItemConfirm(null);
          setDeleteError('');
        }}
        errorMessage={deleteError}
      />
    </div>
  );
};
