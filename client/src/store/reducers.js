import { combineReducers } from 'redux'
import locationReducer from '../commonModules/location'
import authenticationReducer from '../commonModules/authentication'
// import alertReducer from '../commonModules/authentication'
import realReducer from '../routes/Real/modules/real'

export const makeRootReducer = (asyncReducers) => {
  return combineReducers({
    location: locationReducer,
    isAuthenticated : authenticationReducer,
    real: realReducer,
    ...asyncReducers
  })
}

export const injectReducer = (store, { key, reducer }) => {
  if (Object.hasOwnProperty.call(store.asyncReducers, key)) return

  store.asyncReducers[key] = reducer
  store.replaceReducer(makeRootReducer(store.asyncReducers))
}

export default makeRootReducer
