import { createBrowserRouter } from 'react-router-dom';
import { DashboardPage } from '../pages/DashboardPage';
import { LoginPage } from '../pages/LoginPage';
import { ProfilePage } from '../pages/ProfilePage';
import { ObservationsPage } from '../pages/ObservationsPage';
import { MedicationsPage } from '../pages/MedicationsPage';
import { AllergiesPage } from '../pages/AllergiesPage';
import { EncountersPage } from '../pages/EncountersPage';
import { DocumentsPage } from '../pages/DocumentsPage';
import { ConsentsPage } from '../pages/ConsentsPage';
import { AuditPage } from '../pages/AuditPage';
import { RootLayout } from '../components/RootLayout';

export const router = createBrowserRouter([
  {
    path: '/login',
    element: <LoginPage />
  },
  {
    path: '/',
    element: <RootLayout />,
    children: [
      { index: true, element: <DashboardPage /> },
      { path: 'profile', element: <ProfilePage /> },
      { path: 'observations', element: <ObservationsPage /> },
      { path: 'medications', element: <MedicationsPage /> },
      { path: 'allergies', element: <AllergiesPage /> },
      { path: 'encounters', element: <EncountersPage /> },
      { path: 'documents', element: <DocumentsPage /> },
      { path: 'consents', element: <ConsentsPage /> },
      { path: 'audit', element: <AuditPage /> }
    ]
  }
]);
