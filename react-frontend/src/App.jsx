import React from 'react'
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom'
import { AuthProvider, useAuth } from './context/AuthContext'
import { useTokenAutoRefresh } from './hooks/useTokenAutoRefresh'
import Navigation from './components/Navigation'
import Loading from './components/Loading'

// Pages
import HomePage from './pages/HomePage'
import LoginPage from './pages/LoginPage'
import SignupPage from './pages/SignupPage'
// Cars
import CarsPage from './pages/CarsPage'
import CarDetailsPage from './pages/CarDetailsPage'
import CarCreatePage from './pages/CarCreatePage'
import CarEditPage from './pages/CarEditPage'
// Manufacturers
import ManufacturersPage from './pages/ManufacturersPage'
import ManufacturerDetailsPage from './pages/ManufacturerDetailsPage'
import ManufacturerCreatePage from './pages/ManufacturerCreatePage'
import ManufacturerEditPage from './pages/ManufacturerEditPage'
// Engines
import EnginesPage from './pages/EnginesPage'
import EngineDetailsPage from './pages/EngineDetailsPage'
import EngineCreatePage from './pages/EngineCreatePage'
import EngineEditPage from './pages/EngineEditPage'
// Car Profiles
import CarProfilesPage from './pages/CarProfilesPage'
import CarProfileDetailsPage from './pages/CarProfileDetailsPage'
import CarProfileCreatePage from './pages/CarProfileCreatePage'
import CarProfileEditPage from './pages/CarProfileEditPage'
// Car Engines
import CarEnginesPage from './pages/CarEnginesPage'
import CarEngineDetailsPage from './pages/CarEngineDetailsPage'
import CarEngineCreatePage from './pages/CarEngineCreatePage'
import CarEngineEditPage from './pages/CarEngineEditPage'

const ProtectedRoute = ({ children }) => {
  const { isAuthenticated, loading } = useAuth()

  if (loading) {
    return <Loading />
  }

  return isAuthenticated ? children : <Navigate to="/login" replace />
}

const AdminRoute = ({ children }) => {
  const { isAuthenticated, isAdmin, isEmployee, loading } = useAuth()

  if (loading) {
    return <Loading />
  }

  return isAuthenticated && (isAdmin || isEmployee) ? children : <Navigate to="/login" replace />
}

function AppContent() {
  const { loading, isAuthenticated } = useAuth()
  useTokenAutoRefresh(isAuthenticated)

  if (loading) {
    return <Loading />
  }

  return (
    <>
      <Navigation />
      <Routes>
        {/* Public Routes */}
        <Route path="/" element={<HomePage />} />
        <Route path="/login" element={<LoginPage />} />
        <Route path="/signup" element={<SignupPage />} />

        {/* Cars Routes */}
        <Route
          path="/cars"
          element={
            <ProtectedRoute>
              <CarsPage />
            </ProtectedRoute>
          }
        />
        <Route
          path="/cars/:id"
          element={
            <ProtectedRoute>
              <CarDetailsPage />
            </ProtectedRoute>
          }
        />
        <Route
          path="/cars/new"
          element={
            <AdminRoute>
              <CarCreatePage />
            </AdminRoute>
          }
        />
        <Route
          path="/cars/:id/edit"
          element={
            <AdminRoute>
              <CarEditPage />
            </AdminRoute>
          }
        />

        {/* Manufacturers Routes */}
        <Route
          path="/manufacturers"
          element={
            <ProtectedRoute>
              <ManufacturersPage />
            </ProtectedRoute>
          }
        />
        <Route
          path="/manufacturers/:id"
          element={
            <ProtectedRoute>
              <ManufacturerDetailsPage />
            </ProtectedRoute>
          }
        />
        <Route
          path="/manufacturers/new"
          element={
            <AdminRoute>
              <ManufacturerCreatePage />
            </AdminRoute>
          }
        />
        <Route
          path="/manufacturers/:id/edit"
          element={
            <AdminRoute>
              <ManufacturerEditPage />
            </AdminRoute>
          }
        />

        {/* Engines Routes */}
        <Route
          path="/engines"
          element={
            <ProtectedRoute>
              <EnginesPage />
            </ProtectedRoute>
          }
        />
        <Route
          path="/engines/:id"
          element={
            <ProtectedRoute>
              <EngineDetailsPage />
            </ProtectedRoute>
          }
        />
        <Route
          path="/engines/new"
          element={
            <AdminRoute>
              <EngineCreatePage />
            </AdminRoute>
          }
        />
        <Route
          path="/engines/:id/edit"
          element={
            <AdminRoute>
              <EngineEditPage />
            </AdminRoute>
          }
        />

        {/* Car Profiles Routes */}
        <Route
          path="/car-profiles"
          element={
            <ProtectedRoute>
              <CarProfilesPage />
            </ProtectedRoute>
          }
        />
        <Route
          path="/car-profiles/:id"
          element={
            <ProtectedRoute>
              <CarProfileDetailsPage />
            </ProtectedRoute>
          }
        />
        <Route
          path="/car-profiles/new"
          element={
            <AdminRoute>
              <CarProfileCreatePage />
            </AdminRoute>
          }
        />
        <Route
          path="/car-profiles/:id/edit"
          element={
            <AdminRoute>
              <CarProfileEditPage />
            </AdminRoute>
          }
        />

        {/* Car Engines Routes */}
        <Route
          path="/car-engines"
          element={
            <ProtectedRoute>
              <CarEnginesPage />
            </ProtectedRoute>
          }
        />
        <Route
          path="/car-engines/:carId/:engineId"
          element={
            <ProtectedRoute>
              <CarEngineDetailsPage />
            </ProtectedRoute>
          }
        />
        <Route
          path="/car-engines/new"
          element={
            <AdminRoute>
              <CarEngineCreatePage />
            </AdminRoute>
          }
        />
        <Route
          path="/car-engines/:carId/:engineId/edit"
          element={
            <AdminRoute>
              <CarEngineEditPage />
            </AdminRoute>
          }
        />

        {/* Catch all */}
        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </>
  )
}

function App() {
  return (
    <Router>
      <AuthProvider>
        <AppContent />
      </AuthProvider>
    </Router>
  )
}

export default App
