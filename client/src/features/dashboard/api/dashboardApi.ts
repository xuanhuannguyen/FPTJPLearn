import { apiClient } from '../../../shared/api/axios';

export interface DashboardTrustSummary {
  activeUsers: number;
  paidLearners: number;
  contentItems: number;
  recentBuyers: {
    buyer: string;
    packageName: string;
    time: string;
  }[];
}

export const dashboardApi = {
  getTrustSummary: async (): Promise<DashboardTrustSummary> => {
    const response = await apiClient.get<DashboardTrustSummary>('/dashboard/trust-summary');
    return response.data;
  },
};
