import { useEffect, useState } from 'react';
import { useParams, useNavigate, useSearchParams } from 'react-router-dom';
import { ArrowLeft, Trash2, Plus, Play, Loader2, BookOpen, Search } from 'lucide-react';
import { vocabularyApi } from './api/vocabularyApi';
import { AddWordModal } from './components/AddWordModal';
import { ConfirmModal } from '../../shared/components/ConfirmModal';
import { ReviewWorkspace } from '../review/ReviewWorkspace';

const levelBadgeStyles = [
  'bg-slate-200 text-slate-700',
  'bg-amber-100 text-amber-700',
  'bg-orange-100 text-orange-700',
  'bg-sky-100 text-sky-700',
  'bg-emerald-100 text-emerald-700',
  'bg-emerald-200 text-emerald-900',
];

export const VocabularyDetailPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [searchParams, setSearchParams] = useSearchParams();
  const [list, setList] = useState<any>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [isDeleting, setIsDeleting] = useState(false);
  const [deletingItemId, setDeletingItemId] = useState<string | null>(null);
  const [deleteError, setDeleteError] = useState('');
  const [isAddWordOpen, setIsAddWordOpen] = useState(false);
  const [isReviewWorkspaceOpen, setIsReviewWorkspaceOpen] = useState(false);
  const [wordSearchQuery, setWordSearchQuery] = useState('');
  
  // Confirmation states
  const [deleteListConfirm, setDeleteListConfirm] = useState(false);
  const [deleteItemConfirm, setDeleteItemConfirm] = useState<string | null>(null);

  const fetchDetail = async () => {
    try {
      if (!id) return;
      setIsLoading(true);
      const data = await vocabularyApi.getListById(id);
      setList(data);
    } catch (err) {
      console.error(err);
      navigate('/vocabulary'); // Go back if error or not found
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchDetail();
  }, [id]);

  useEffect(() => {
    if (searchParams.get('study') === '1') {
      setIsReviewWorkspaceOpen(true);
    }
  }, [searchParams]);

  const executeDeleteList = async () => {
    try {
      setIsDeleting(true);
      setDeleteError('');
      await vocabularyApi.deleteList(id!);
      navigate('/vocabulary');
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
      setList((prev: any) => ({
        ...prev,
        items: prev.items.filter((i: any) => i.id !== itemId),
        wordCount: Math.max(0, prev.wordCount - 1)
      }));
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

  const filteredItems = list.items.filter((item: any) => {
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
          <div className="flex items-center gap-4 text-text-secondary mb-4">
            <button 
              onClick={() => navigate('/vocabulary')}
              className="hover:text-accent-primary transition-colors flex items-center gap-2 font-medium"
            >
              <ArrowLeft size={20} />
              Back to Lists
            </button>
          </div>

          <div className="flex flex-col md:flex-row md:items-end justify-between gap-6 bg-bg-secondary p-8 rounded-3xl border border-border shadow-card relative">
            {/* Decorative background element */}
            <div className="absolute inset-0 overflow-hidden rounded-3xl pointer-events-none">
              <div className="absolute top-0 right-0 w-64 h-64 bg-accent-primary/5 rounded-full blur-3xl -translate-y-1/2 translate-x-1/3" />
            </div>

            <div className="relative z-10">
              <h1 className="text-3xl md:text-4xl font-bold text-text-primary mb-3">{list.name}</h1>
              <p className="text-text-secondary text-lg max-w-2xl">{list.description}</p>
              <div className="flex items-center gap-4 mt-6">
                <div className="bg-bg-tertiary px-4 py-1.5 rounded-full text-sm font-medium text-text-primary">
                  {list.wordCount} words
                </div>
                <div className="bg-accent-primary/10 text-accent-primary px-4 py-1.5 rounded-full text-sm font-medium">
                  Created {new Date(list.createdAt).toLocaleDateString()}
                </div>
              </div>
            </div>

            <div className="flex items-center gap-3 relative z-10">
              <button 
                onClick={() => {
                  setDeleteError('');
                  setDeleteListConfirm(true);
                }}
                disabled={isDeleting}
                className="p-3 text-accent-danger hover:bg-accent-danger/10 rounded-xl transition-colors disabled:opacity-50"
                title="Delete List"
              >
                <Trash2 size={20} />
              </button>
              
              <button 
                onClick={() => {
                  setIsReviewWorkspaceOpen(true);
                  setSearchParams({ study: '1' });
                }}
                className="bg-accent-primary hover:bg-accent-hover text-white font-semibold px-6 py-3 rounded-xl transition-all shadow-glow hover:-translate-y-0.5 flex items-center gap-2"
              >
                <Play size={20} className="fill-current" />
                <span>Study Now</span>
              </button>
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
          <div className="flex flex-col gap-4 lg:flex-row lg:items-center lg:justify-between">
            <div className="flex flex-col gap-3 md:flex-row md:items-center">
              <h2 className="text-xl font-bold text-text-primary flex items-center gap-2">
                <BookOpen size={24} className="text-accent-primary" />
                Vocabulary Words
              </h2>
              <div className="relative w-full md:w-80">
                <Search className="absolute left-3 top-1/2 -translate-y-1/2 text-text-muted" size={18} />
                <input
                  type="text"
                  value={wordSearchQuery}
                  onChange={(event) => setWordSearchQuery(event.target.value)}
                  placeholder="Search words in this list..."
                  className="w-full rounded-xl border border-border bg-bg-secondary pl-10 pr-4 py-2.5 text-sm text-text-primary outline-none transition-colors focus:border-accent-primary focus:ring-1 focus:ring-accent-primary placeholder:text-text-muted"
                />
              </div>
            </div>
            <button 
              onClick={() => setIsAddWordOpen(true)}
              className="flex items-center gap-2 text-accent-primary font-semibold hover:bg-accent-primary/10 px-4 py-2 rounded-xl transition-colors"
            >
              <Plus size={20} />
              Add Word
            </button>
          </div>

          {/* Words List */}
          <div className="bg-bg-secondary border border-border rounded-2xl overflow-hidden shadow-sm">
            <div className="divide-y divide-border">
              {filteredItems.map((item: any, index: number) => (
                <div key={item.id} className="p-4 md:p-6 hover:bg-bg-tertiary/50 transition-colors flex items-center justify-between gap-4 group">
                  
                  <div className="flex items-start gap-4 flex-1">
                    <div className="text-text-muted font-mono w-8 mt-1">{index + 1}.</div>
                    
                    <div className="flex-1 grid grid-cols-1 md:grid-cols-2 gap-4">
                      {/* Japanese Side */}
                      <div>
                        <div className="flex items-end gap-3 mb-1">
                          <span className="text-2xl font-bold font-heading text-text-primary">{item.word}</span>
                          <span className="text-text-secondary pb-0.5">{item.reading}</span>
                        </div>
                        <span className="inline-block px-2 py-0.5 bg-bg-tertiary text-xs text-text-secondary rounded">
                          {item.wordType}
                        </span>
                      </div>

                      {/* Meaning Side */}
                      <div>
                        <p className="text-lg font-medium text-text-primary mb-1">{item.meaning}</p>
                        {item.exampleSentence && (
                          <div className="text-sm mt-2">
                            <p className="text-text-secondary font-medium">{item.exampleSentence}</p>
                            <p className="text-text-muted">{item.exampleMeaning}</p>
                          </div>
                        )}
                      </div>
                    </div>
                  </div>

                  {/* Actions & Memorization Level */}
                  <div className="flex flex-col items-end gap-3">
                    <div
                      className={`rounded-full px-3 py-1 text-xs font-semibold ${levelBadgeStyles[item.level] || levelBadgeStyles[0]}`}
                      title={`Level ${item.level}`}
                    >
                      Level {item.level}
                    </div>
                    <div className="flex items-center gap-2">
                      <div className={`px-2 py-0.5 rounded text-[10px] font-bold uppercase tracking-wider ${
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
                        className="p-1.5 text-text-muted hover:text-accent-danger hover:bg-accent-danger/10 rounded-md transition-colors opacity-100 md:opacity-0 md:group-hover:opacity-100 disabled:cursor-not-allowed disabled:opacity-50"
                        title="Remove Word"
                      >
                        <Trash2 size={16} />
                      </button>
                    </div>
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
        onSuccess={(newItem) => {
          setList((prev: any) => ({
            ...prev,
            items: [...prev.items, newItem],
            wordCount: prev.wordCount + 1
          }));
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
