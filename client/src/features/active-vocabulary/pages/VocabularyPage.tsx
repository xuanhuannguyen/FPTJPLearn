import { BookOpen, CheckCircle, Clock, Play, Loader2, AlertCircle, Trash2, Search } from 'lucide-react';
import { useNavigate } from 'react-router-dom';
import { useState, useEffect, useMemo, useCallback } from 'react';
import { ImportModal } from '../components/ImportModal';
import { vocabularyApi } from '../api/vocabularyApi';
import type { VocabularyList } from '../api/vocabularyApi';
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

  const fetchLists = useCallback(async () => {
    try {
      setIsLoading(true);
      setError('');
      const data = await vocabularyApi.getLists();
      setLists(data);
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

    const missingListIds = lists
      .map((list) => list.id)
      .filter((listId) => !listSearchIndex[listId]);

    if (missingListIds.length === 0) {
      return;
    }

    let active = true;

    const buildSearchIndex = async () => {
      try {
        setIsSearchIndexLoading(true);
        const details = await Promise.all(missingListIds.map((listId) => vocabularyApi.getListById(listId)));

        if (!active) {
          return;
        }

        setListSearchIndex((prev) => {
          const next = { ...prev };
          details.forEach((detail) => {
            next[detail.id] = detail.items.map((item) => ({
              id: item.id,
              word: item.word || '',
              reading: item.reading || '',
              meaning: item.meaning || '',
              wordType: item.wordType || '',
            }));
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

    void buildSearchIndex();

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
      <div className="relative rounded-[24px] border-2 border-border bg-white/85 px-5 py-5 shadow-card backdrop-blur-sm md:px-7">
        <div className="grid gap-5 lg:grid-cols-[minmax(0,1fr)_auto] lg:items-center">
        <div className="min-w-0">
          <p className="text-xs font-black uppercase leading-none tracking-[0.18em] text-text-secondary">Smart vocabulary</p>
          <div className="mt-2 inline-flex max-w-full rounded-2xl bg-[#f3d6ff] px-4 py-2">
            <h1 className="truncate font-heading text-4xl font-black leading-none tracking-tight text-text-primary md:text-5xl">
              Từ vựng chủ động
            </h1>
          </div>
          <p className="mt-2 max-w-2xl text-base font-bold leading-5 text-text-secondary">Quản lý bộ từ riêng của bạn, nhập JSON và ôn tập theo SRS.</p>
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
        <div className="grid grid-cols-1 gap-5 lg:grid-cols-2 xl:grid-cols-3">
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
              className="interactive-surface group relative flex cursor-pointer gap-4 overflow-hidden rounded-[18px] p-3.5"
            >
              <div className={`absolute left-0 top-0 h-full w-2 ${isComplete ? 'bg-accent-success' : 'bg-accent-primary'}`} />
              <div className="book-cover relative flex h-20 w-16 shrink-0 items-center justify-center rounded-lg">
                <div className="flex h-10 w-10 items-center justify-center rounded-full bg-white text-center font-heading text-[8px] font-black leading-none text-text-primary">
                  TỪ<br />VỰNG
                </div>
              </div>

              <div className="relative flex min-w-0 flex-1 flex-col justify-center">
                <div className="flex items-start justify-between gap-3">
                  <div className="min-w-0 flex-1">
                    <div className="flex items-center gap-2">
                      <h3 className="line-clamp-1 font-heading text-xl font-black leading-none tracking-tight text-text-primary transition-colors group-hover:text-accent-primary">
                        {list.name}
                      </h3>
                      {isComplete && (
                        <CheckCircle className="text-accent-success shrink-0" size={18} />
                      )}
                    </div>
                    <div className="mt-2 flex items-center gap-2 text-[11px] font-bold">
                      <div className="flex items-center gap-1.5 rounded-lg border border-border/40 bg-bg-tertiary px-2 py-0.5 text-text-primary">
                        <BookOpen size={12} className="text-text-muted" />
                        <span className="font-black leading-none">{list.wordCount}</span>
                      </div>
                      <div className="flex items-center gap-1.5 rounded-lg border border-accent-success/20 bg-accent-success/5 px-2 py-0.5 text-accent-success">
                        <CheckCircle size={12} />
                        <span className="font-black leading-none">{list.masteredCount}</span>
                      </div>
                      {list.dueCount > 0 && (
                        <div className="flex items-center gap-1.5 rounded-lg border border-accent-warning/20 bg-accent-warning/5 px-2 py-0.5 text-accent-warning">
                          <Clock size={12} />
                          <span className="font-black leading-none">{list.dueCount}</span>
                        </div>
                      )}
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
                      <Trash2 size={16} />
                    </button>
                    <button 
                      onClick={(event) => {
                        event.stopPropagation();
                        navigate(`/active-vocabulary/${list.id}?study=1`);
                      }}
                      className={`flex h-8 w-8 items-center justify-center rounded-lg border-2 border-border transition-all ${
                        list.dueCount > 0 
                          ? 'bg-accent-cta text-white shadow-pop hover:-translate-y-0.5' 
                          : 'bg-bg-tertiary text-text-secondary hover:text-accent-primary cursor-pointer'
                      }`}
                    >
                      <Play size={14} className={list.dueCount > 0 ? 'ml-0.5' : ''} />
                    </button>
                  </div>
                </div>



                {/* Progress Bar - Simplified */}
                <div className="mt-3 flex items-center gap-3">
                  <div className="h-1.5 flex-1 overflow-hidden rounded-full border border-border bg-bg-tertiary">
                    <div 
                      className={`h-full rounded-full transition-all duration-500 ${isComplete ? 'bg-accent-success' : 'bg-accent-primary'}`}
                      style={{ width: `${progressPercent}%` }}
                    />
                  </div>
                  <span className="text-[10px] font-black text-text-primary tabular-nums">{progressPercent}%</span>
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
