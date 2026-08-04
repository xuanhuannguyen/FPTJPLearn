import { BookOpen, CheckCircle, Clock, Play, Loader2, AlertCircle, Trash2, Search } from 'lucide-react';
import { useNavigate } from 'react-router-dom';
import { useState, useEffect, useMemo, useCallback } from 'react';
import { ImportModal } from '../components/ImportModal';
import { vocabularyApi } from '../api/vocabularyApi';
import type { VocabularyList, VocabularyQuota } from '../api/vocabularyApi';
import { ConfirmModal } from '../../../shared/components/ConfirmModal';
import { useSearchStore } from '../../../shared/stores/searchStore';

type SearchableVocabularyItem = {
  id: string;
  word: string;
  reading: string;
  meaning: string;
  wordType: string;
};

export const VocabularyPage = () => {
  const navigate = useNavigate();
  const [isImportOpen, setIsImportOpen] = useState(false);
  const [lists, setLists] = useState<VocabularyList[]>([]);
  const [listSearchIndex, setListSearchIndex] = useState<Record<string, SearchableVocabularyItem[]>>({});
  const [isLoading, setIsLoading] = useState(true);
  const [isSearchIndexLoading, setIsSearchIndexLoading] = useState(false);
  const [error, setError] = useState('');
  const [deleteListConfirm, setDeleteListConfirm] = useState<string | null>(null);
  const [deleteError, setDeleteError] = useState('');
  const searchQuery = useSearchStore((state) => state.query);
  const setSearchQuery = useSearchStore((state) => state.setQuery);

  const [quota, setQuota] = useState<VocabularyQuota | null>(null);

  const fetchLists = useCallback(async () => {
    try {
      setIsLoading(true);
      setError('');
      const [data, quotaData] = await Promise.all([
        vocabularyApi.getLists(),
        vocabularyApi.getQuota()
      ]);
      setLists(data);
      setQuota(quotaData);
    } catch (err: unknown) {
      setError('Failed to fetch vocabulary lists. Ensure the backend is running.');
      console.error(err);
    } finally {
      setIsLoading(false);
    }
  }, []);

  useEffect(() => {
    const timer = window.setTimeout(() => {
      void fetchLists();
    }, 0);

    return () => window.clearTimeout(timer);
  }, [fetchLists]);

  useEffect(() => {
    const normalizedQuery = searchQuery.trim();
    if (!normalizedQuery || lists.length === 0) {
      return;
    }

    // Only fetch the index once if it hasn't been fetched yet
    if (Object.keys(listSearchIndex).length > 0) {
      return;
    }

    let active = true;

    const fetchSearchIndex = async () => {
      try {
        setIsSearchIndexLoading(true);
        const allItems = await vocabularyApi.getSearchIndex();

        if (!active) {
          return;
        }

        setListSearchIndex(() => {
          const next: Record<string, SearchableVocabularyItem[]> = {};
          
          // Group items by listId
          allItems.forEach((item) => {
            if (!next[item.listId]) {
              next[item.listId] = [];
            }
            next[item.listId].push({
              id: item.id,
              word: item.word || '',
              reading: item.reading || '',
              meaning: item.meaning || '',
              wordType: item.wordType || '',
            });
          });
          
          // Ensure all lists exist in the index even if empty
          lists.forEach(list => {
            if (!next[list.id]) next[list.id] = [];
          });
          
          return next;
        });
      } catch (searchIndexError) {
        console.error('Failed to build vocabulary search index', searchIndexError);
      } finally {
        if (active) {
          setIsSearchIndexLoading(false);
        }
      }
    };

    void fetchSearchIndex();

    return () => {
      active = false;
    };
  }, [lists, listSearchIndex, searchQuery]);

  const matchedWordsByList = useMemo(() => {
    const normalizedQuery = searchQuery.trim().toLowerCase();
    if (!normalizedQuery) {
      return {} as Record<string, SearchableVocabularyItem[]>;
    }

    return lists.reduce<Record<string, SearchableVocabularyItem[]>>((acc, list) => {
      const items = listSearchIndex[list.id] || [];
      const matches = items.filter((item) => {
        const haystack = `${item.word} ${item.reading} ${item.meaning} ${item.wordType}`.toLowerCase();
        return haystack.includes(normalizedQuery);
      });

      if (matches.length > 0) {
        acc[list.id] = matches;
      }

      return acc;
    }, {});
  }, [listSearchIndex, lists, searchQuery]);

  const filteredLists = useMemo(() => {
    const normalizedQuery = searchQuery.trim().toLowerCase();
    if (!normalizedQuery) {
      return lists;
    }

    return lists.filter((list) => {
      const listHaystack = `${list.name} ${list.description}`.toLowerCase();
      if (listHaystack.includes(normalizedQuery)) {
        return true;
      }

      return (matchedWordsByList[list.id] || []).length > 0;
    });
  }, [lists, matchedWordsByList, searchQuery]);

  const executeDeleteList = async (id: string) => {
    try {
      setDeleteError('');
      await vocabularyApi.deleteList(id);
      setLists(prev => prev.filter(l => l.id !== id));
    } catch (err) {
      console.error('Failed to delete list', err);
      setDeleteError('Could not delete this vocabulary list. Please try again.');
      throw err;
    }
  };

  return (
    <div className="space-y-6 animate-fade-in">
      <div className="relative rounded-[20px] border-2 border-border bg-white/85 px-4 py-3 shadow-card backdrop-blur-sm md:px-6">
        <div className="grid gap-4 lg:grid-cols-[minmax(0,1fr)_auto] lg:items-center">
        <div className="min-w-0">
          <p className="text-[10px] font-black uppercase leading-none tracking-[0.18em] text-text-secondary">Smart vocabulary</p>
          <div className="mt-1.5 inline-flex max-w-full rounded-xl bg-[#f3d6ff] px-3 py-1">
            <h1 className="truncate font-heading text-2xl font-black leading-none tracking-tight text-text-primary md:text-3xl">
              Từ vựng chủ động
            </h1>
          </div>
          <p className="mt-1.5 max-w-2xl text-xs font-bold leading-relaxed text-text-secondary">Quản lý bộ từ riêng của bạn, nhập JSON và ôn tập theo SRS.</p>
          {searchQuery.trim() && (
            <p className="mt-2 text-sm font-semibold text-text-muted">
              Đang tìm trong tên bộ, mô tả và từ bên trong từng bộ.
              {isSearchIndexLoading ? ' Đang tải kết quả từ vựng...' : ''}
            </p>
          )}
        </div>
        <div className="flex flex-col gap-3 sm:flex-row sm:items-center lg:shrink-0 lg:justify-end">
        <div className="grid grid-cols-3 gap-2 text-sm lg:min-w-[252px]">
          <div className="rounded-2xl border-2 border-border bg-bg-tertiary px-3 py-2 text-center shadow-pop">
            <div className="text-xl font-black leading-none text-text-primary">{lists.length}</div>
            <div className="mt-1 text-xs font-extrabold text-text-muted">Bộ riêng</div>
          </div>
          <div className="rounded-2xl border-2 border-border bg-white px-3 py-2 text-center shadow-pop">
            <div className="text-xl font-black leading-none text-text-primary">{lists.reduce((sum, list) => sum + list.wordCount, 0)}</div>
            <div className="mt-1 text-xs font-extrabold text-text-muted">Từ</div>
          </div>
          <div className="rounded-2xl border-2 border-border bg-accent-primary px-3 py-2 text-center text-white shadow-pop">
            <div className="text-xl font-black leading-none">{lists.reduce((sum, list) => sum + list.dueCount, 0)}</div>
            <div className="mt-1 text-xs font-extrabold">Cần ôn</div>
          </div>
        </div>
        <div className="flex flex-col items-end gap-1">
          {quota && (
            <div className="text-[10px] font-black uppercase tracking-wider text-text-muted">
              Còn lại: <span className={quota.remainingCount > 0 ? 'text-accent-primary' : 'text-accent-danger'}>
                {quota.remainingCount} lượt
              </span>
              {quota.period === 'daily' && ' (hôm nay)'}
            </div>
          )}
          <button 
            onClick={() => setIsImportOpen(true)}
            className="btn-primary min-h-11 px-4 py-2"
          >
            <BookOpen size={18} />
            <span>Nhập JSON</span>
          </button>
        </div>
        </div>
      </div>
    </div>
      
    <div className="relative mx-auto max-w-2xl">
      <Search className="absolute left-4 top-1/2 -translate-y-1/2 text-text-muted" size={18} />
      <input
        type="text"
        value={searchQuery}
        onChange={(e) => setSearchQuery(e.target.value)}
        placeholder="Tìm kiếm bộ từ hoặc từ vựng..."
        className="h-11 w-full rounded-xl border-2 border-border bg-white px-12 text-sm font-bold shadow-[2px_2px_0px_0px_rgba(0,0,0,1)] transition-all focus:translate-x-[1px] focus:translate-y-[1px] focus:shadow-[1px_1px_0px_0px_rgba(0,0,0,1)] outline-none"
      />
    </div>

      {isLoading ? (
        <div className="flex flex-col items-center justify-center py-20 text-text-secondary">
          <Loader2 size={40} className="animate-spin mb-4 text-accent-primary" />
            <p>Đang tải bộ từ chủ động...</p>
        </div>
      ) : error ? (
        <div className="flex items-center gap-4 rounded-3xl border border-accent-danger/20 bg-accent-danger/10 p-6 text-accent-danger">
          <AlertCircle size={24} />
          <p>{error}</p>
        </div>
      ) : lists.length === 0 ? (
        <div className="glass-card p-12 flex flex-col items-center justify-center text-center">
          <BookOpen size={48} className="text-text-muted mb-4" />
          <h3 className="text-xl font-bold text-text-primary mb-2">No lists found</h3>
          <p className="text-text-secondary mb-6 max-w-md">You haven't imported any vocabulary lists yet. Import a JSON file to get started with your learning journey.</p>
          <button 
            onClick={() => setIsImportOpen(true)}
            className="text-accent-primary font-semibold hover:underline"
          >
            Import your first list
          </button>
        </div>
      ) : filteredLists.length === 0 ? (
        <div className="glass-card p-12 flex flex-col items-center justify-center text-center">
          <BookOpen size={48} className="text-text-muted mb-4" />
          <h3 className="text-xl font-bold text-text-primary mb-2">No matching lists</h3>
          <p className="text-text-secondary max-w-md">
            No vocabulary list matches "<span className="font-medium">{searchQuery}</span>".
          </p>
        </div>
      ) : (
        <div className="grid grid-cols-1 gap-4 md:grid-cols-2 xl:grid-cols-4">
          {filteredLists.map((list) => {
            const progressPercent = list.wordCount > 0 
              ? Math.round((list.masteredCount / list.wordCount) * 100) 
              : 0;
            const isComplete = list.wordCount > 0 && progressPercent === 100;

            return (
            <div
              key={list.id}
              onClick={() => navigate(`/active-vocabulary/${list.id}`)}
              onKeyDown={(event) => {
                if (event.key === 'Enter' || event.key === ' ') {
                  event.preventDefault();
                  navigate(`/active-vocabulary/${list.id}`);
                }
              }}
              role="button"
              tabIndex={0}
              className="group relative flex min-h-[110px] cursor-pointer gap-3.5 overflow-hidden rounded-[18px] border-2 border-slate-900 bg-white p-3.5 shadow-[4px_4px_0_#111827] transition-all hover:translate-x-[1px] hover:translate-y-[1px] hover:shadow-[2px_2px_0_#111827]"
            >
              <div className={`absolute left-0 top-0 h-full w-1.5 ${isComplete ? 'bg-accent-success' : 'bg-blue-500'}`} />

              <img
                src="/images/vocabulary/vocabulary-card.webp?v=2"
                alt=""
                className="h-20 w-16 shrink-0 rounded-lg border-2 border-slate-900 object-cover shadow-[2px_2px_0_#111827]"
                loading="lazy"
                decoding="async"
              />

              <div className="relative flex min-w-0 flex-1 flex-col justify-between">
                <div className="flex min-w-0 items-start justify-between gap-2">
                  <div className="min-w-0">
                    <div className="flex items-center gap-1.5">
                      <h3 className="line-clamp-1 font-heading text-base font-black leading-tight text-slate-950 transition-colors group-hover:text-blue-600">
                        {list.name}
                      </h3>
                      {isComplete && (
                        <CheckCircle className="shrink-0 text-accent-success" size={15} />
                      )}
                    </div>
                    <div className="mt-1.5 flex flex-wrap items-center gap-1.5">
                      <span className="inline-flex h-5 items-center gap-1 rounded-full border border-slate-200 bg-slate-50 px-2 text-[10px] font-black text-slate-600">
                        <BookOpen size={11} />
                        {list.wordCount}
                      </span>
                      <span className="inline-flex h-5 items-center gap-1 rounded-full border border-accent-success/20 bg-accent-success/5 px-2 text-[10px] font-black text-accent-success">
                        <CheckCircle size={11} />
                        {list.masteredCount}
                      </span>
                      <span className="inline-flex h-5 items-center gap-1 rounded-full border border-accent-warning/20 bg-accent-warning/5 px-2 text-[10px] font-black text-accent-warning">
                        <Clock size={11} />
                        {list.dueCount}
                      </span>
                    </div>
                  </div>

                  <div className="flex shrink-0 items-center gap-1">
                    <button
                      onClick={(e) => {
                        e.stopPropagation();
                        setDeleteError('');
                        setDeleteListConfirm(list.id);
                      }}
                      className="rounded-lg p-1.5 text-text-muted opacity-100 transition-colors hover:bg-accent-danger/10 hover:text-accent-danger md:opacity-0 md:group-hover:opacity-100"
                      title="Delete List"
                    >
                      <Trash2 size={14} />
                    </button>
                    <button
                      onClick={(event) => {
                        event.stopPropagation();
                        navigate(`/active-vocabulary/${list.id}?study=1`);
                      }}
                      className={`flex h-8 w-8 items-center justify-center rounded-lg border-2 border-slate-900 transition-all ${
                        list.dueCount > 0
                          ? 'bg-orange-600 text-white shadow-[2px_2px_0_#111827] hover:bg-orange-500'
                          : 'bg-bg-tertiary text-text-secondary hover:text-accent-primary'
                      }`}
                    >
                      <Play size={14} className={list.dueCount > 0 ? 'ml-0.5 fill-current' : ''} />
                    </button>
                  </div>
                </div>

                <div className="mt-2 flex items-center gap-2">
                  <div className="h-1.5 flex-1 overflow-hidden rounded-full border border-slate-900 bg-blue-50">
                    <div
                      className={`h-full rounded-full transition-all duration-500 ${isComplete ? 'bg-accent-success' : 'bg-blue-600'}`}
                      style={{ width: `${progressPercent}%` }}
                    />
                  </div>
                  <span className="w-7 text-right text-[10px] font-black text-slate-900 tabular-nums">{progressPercent}%</span>
                </div>
              </div>
            </div>
          );
        })}
      </div>
      )}

      <ImportModal 
        isOpen={isImportOpen} 
        onClose={() => setIsImportOpen(false)} 
        onSuccess={() => fetchLists()} 
      />

      <ConfirmModal 
        isOpen={!!deleteListConfirm}
        title="Delete Vocabulary List"
        message="Are you sure you want to delete this list? All progress and words will be lost."
        confirmText="Delete List"
        onConfirm={() => {
          if (deleteListConfirm) {
            return executeDeleteList(deleteListConfirm);
          }
        }}
        onCancel={() => {
          setDeleteListConfirm(null);
          setDeleteError('');
        }}
        errorMessage={deleteError}
      />
    </div>
  );
};
