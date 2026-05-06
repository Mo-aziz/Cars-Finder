import React, { useState, useEffect } from 'react'
import { useParams, Link } from 'react-router-dom'
import { carProfileService } from '../services/carProfileService'
import Loading from '../components/Loading'
import Alert from '../components/Alert'

const CarProfileDetailsPage = () => {
  const { id } = useParams()
  const [profile, setProfile] = useState(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const defaultCarIcon = '/default-car.svg'

  useEffect(() => {
    fetchProfile()
  }, [id])

  const fetchProfile = async () => {
    try {
      setLoading(true)
      const data = await carProfileService.getById(id)
      setProfile(data)
    } catch (err) {
      setError(err.message || 'Failed to fetch car profile')
    } finally {
      setLoading(false)
    }
  }

  if (loading) return <Loading />

  if (!profile) {
    return (
      <div className="container">
        <Alert message="Car profile not found" type="error" />
        <Link to="/car-profiles" className="btn-secondary inline-block">
          Back to Car Profiles
        </Link>
      </div>
    )
  }

  return (
    <div className="container">
      <Link to="/car-profiles" className="text-blue-400 hover:text-blue-300 mb-4 inline-block">
        ← Back to Car Profiles
      </Link>

      <Alert message={error} type="error" onClose={() => setError('')} />

      <div className="card max-w-2xl">
        <h1 className="text-4xl font-bold mb-6 text-blue-400">
          {profile.carName || `Car ID: ${profile.carId}`}
        </h1>

        <div className="mb-6">
          <img
            src={profile.photoUrl || defaultCarIcon}
            alt={profile.carName || 'Car profile image'}
            className="w-full max-w-md h-64 rounded object-cover border border-gray-700"
            onError={(e) => {
              e.currentTarget.onerror = null
              e.currentTarget.src = defaultCarIcon
            }}
          />
        </div>
        
        <div className="space-y-4">
          <div className="border-b border-gray-700 pb-3 transition duration-300">
            <p className="text-gray-400 text-sm uppercase tracking-wide">Color</p>
            <p className="text-2xl font-semibold">{profile.color}</p>
          </div>

          <div className="border-b border-gray-700 pb-3 transition duration-300">
            <p className="text-gray-400 text-sm uppercase tracking-wide">Price</p>
            <p className="text-2xl font-semibold">${profile.price?.toLocaleString() || 'N/A'}</p>
          </div>

          {profile.description && (
            <div className="transition duration-300">
              <p className="text-gray-400 text-sm uppercase tracking-wide">Description</p>
              <p className="text-lg mt-2">{profile.description}</p>
            </div>
          )}
        </div>
      </div>
    </div>
  )
}

export default CarProfileDetailsPage
