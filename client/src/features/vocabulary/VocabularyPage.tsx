import { BookOpen, CheckCircle, Clock, Play, Loader2, AlertCircle, Trash2 } from 'lucide-react';
import { useNavigate } from 'react-router-dom';
import { useState, useEffect, useMemo } from 'react';
import { ImportModal } from './components/ImportModal';
import { vocabularyApi } from './api/vocabularyApi';
import { ConfirmModal } from '../../shared/components/ConfirmModal';
import { useSearchStore } from '../../shared/stores/searchStore';

export interface VocabularyList {
  id: string;
  name: string;
  description: string;
  wordCount: number;
  masteredCount: number;
  dueCount: number;
  createdAt: string;
}

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

  const fetchLists = async () => {
    try {
      setIsLoading(true);
      setError('');
      const data = await vocabularyApi.getLists();
      setLists(data);
    } catch (err: any) {
      setError('Failed to fetch vocabulary lists. Ensure the backend is running.');
      console.error(err);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchLists();
  }, []);

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
            next[detail.id] = detail.items.map((item: any) => ({
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
    <div className="space-y-8 animate-fade-in">
      <div className="flex flex-col md:flex-row md:items-end justify-between gap-4">
        <div>
          <h1 className="text-3xl font-bold text-text-primary mb-2">Vocabulary Lists</h1>
          <p className="text-text-secondary">Manage your learning lists and track SRS progress.</p>
          {searchQuery.trim() && (
            <p className="mt-2 text-sm text-text-muted">
              Searching list names, descriptions, and words inside each list.
              {isSearchIndexLoading ? ' Loading vocabulary matches...' : ''}
            </p>
          )}
        </div>
        <button 
          onClick={() => setIsImportOpen(true)}
          className="bg-accent-primary hover:bg-accent-hover text-white font-semibold px-6 py-2.5 rounded-xl transition-all shadow-glow hover:-translate-y-0.5 flex items-center gap-2"
        >
          <BookOpen size={18} />
          <span>Import JSON</span>
        </button>
      </div>

      {isLoading ? (
        <div className="flex flex-col items-center justify-center py-20 text-text-secondary">
          <Loader2 size={40} className="animate-spin mb-4 text-accent-primary" />
          <p>Loading your vocabulary lists...</p>
        </div>
      ) : error ? (
        <div className="bg-accent-danger/10 border border-accent-danger/20 text-accent-danger p-6 rounded-2xl flex items-center gap-4">
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
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {filteredLists.map((list) => {
            const progressPercent = list.wordCount > 0 
              ? Math.round((list.masteredCount / list.wordCount) * 100) 
              : 0;
            const isComplete = list.wordCount > 0 && progressPercent === 100;

            return (
            <div 
              key={list.id} 
              onClick={() => navigate(`/vocabulary/${list.id}`)}
              onKeyDown={(event) => {
                if (event.key === 'Enter' || event.key === ' ') {
                  event.preventDefault();
                  navigate(`/vocabulary/${list.id}`);
                }
              }}
              role="button"
              tabIndex={0}
              className="glass-card p-6 flex flex-col group hover:-translate-y-1 transition-all duration-300 relative overflow-hidden cursor-pointer"
            >
              {/* Top Accent Line */}
              <div className={`absolute top-0 left-0 w-full h-1 ${isComplete ? 'bg-accent-success' : 'bg-accent-primary opacity-50'}`} />

              <div className="flex justify-between items-start mb-4">
                <div className="flex-1">
                  <h3 className="text-xl font-bold text-text-primary group-hover:text-accent-primary transition-colors pr-2">
                    {list.name}
                  </h3>
                  <p className="text-text-secondary text-sm mt-1 line-clamp-1">{list.description}</p>
                  {matchedWordsByList[list.id]?.length ? (
                    <div className="mt-3 flex flex-wrap gap-2">
                      {matchedWordsByList[list.id].slice(0, 2).map((item) => (
                        <span
                          key={item.id}
                          className="rounded-full bg-accent-primary/10 px-2.5 py-1 text-xs font-medium text-accent-primary"
                        >
                          {item.word} · {item.meaning}
                        </span>
                      ))}
                      {matchedWordsByList[list.id].length > 2 && (
                        <span className="rounded-full bg-bg-tertiary px-2.5 py-1 text-xs font-medium text-text-secondary">
                          +{matchedWordsByList[list.id].length - 2} more
                        </span>
                      )}
                    </div>
                  ) : null}
                </div>
                <div className="flex items-center gap-2">
                  {isComplete && (
                    <CheckCircle className="text-accent-success shrink-0" size={24} />
                  )}
                  <button 
                    onClick={(e) => {
                      e.stopPropagation();
                      setDeleteError('');
                      setDeleteListConfirm(list.id);
                    }}
                    className="p-1.5 text-text-muted hover:text-accent-danger hover:bg-accent-danger/10 rounded-lg transition-colors opacity-100 md:opacity-0 md:group-hover:opacity-100"
                    title="Delete List"
                  >
                    <Trash2 size={18} />
                  </button>
                </div>
              </div>

              <div className="mt-auto pt-6 space-y-4">
                {/* Progress Bar */}
                <div className="space-y-2">
                  <div className="flex justify-between text-xs font-medium">
                    <span className="text-text-secondary">Progress</span>
                    <span className="text-text-primary">{progressPercent}%</span>
                  </div>
                  <div className="h-2 w-full bg-bg-tertiary rounded-full overflow-hidden">
                    <div 
                      className={`h-full rounded-full transition-all duration-500 ${isComplete ? 'bg-accent-success' : 'bg-accent-primary'}`}
                      style={{ width: `${progressPercent}%` }}
                    />
                  </div>
                </div>

                {/* Status & Action */}
                <div className="flex items-center justify-between pt-2 border-t border-border/50">
                  <div className="flex items-center gap-2 text-sm">
                    <Clock size={16} className={list.dueCount > 0 ? 'text-accent-warning' : 'text-text-muted'} />
                    <span className={list.dueCount > 0 ? 'text-accent-warning font-medium' : 'text-text-muted'}>
                      {list.dueCount > 0 ? `${list.dueCount} reviews due` : 'All caught up'}
                    </span>
                  </div>
                  
                  <button 
                    onClick={(event) => {
                      event.stopPropagation();
                      navigate(`/vocabulary/${list.id}?study=1`);
                    }}
                    className={`w-10 h-10 rounded-full flex items-center justify-center transition-all ${
                      list.dueCount > 0 
                        ? 'bg-accent-primary text-white hover:scale-110 shadow-glow' 
                        : isComplete 
                          ? 'bg-bg-tertiary text-text-secondary hover:text-accent-primary cursor-pointer'
                          : 'bg-bg-tertiary text-text-secondary hover:text-accent-primary cursor-pointer'
                    }`}
                  >
                    <Play size={18} className={list.dueCount > 0 ? 'ml-0.5' : ''} />
                  </button>
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
