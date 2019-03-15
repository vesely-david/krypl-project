import {
  LOGIN,
  LOGOUT
} from '../actions/types';


const ACTION_HANDLERS = {
  [`${LOGIN}_PENDING`] : (state, action) => {
    return {...state, isFetching: true}
  },
  [`${LOGIN}_FULFILLED`] : (state, action) => {
    return {...state, isAuthenticated: true, email: 'return in future', isFetching: false}
  },
  [`${LOGIN}_REJECTED`] : (state, action) => ({...state, isFetching: false}),
  [LOGOUT] : (state, action) => ({...state, isAuthenticated: false}),
}

const initialState = {
  email: '',
  isAuthenticated: !!localStorage.getItem('token'),
  isFetching: false,
}

export default function userReducer (state = initialState, action) {
  const handler = ACTION_HANDLERS[action.type]
  return handler ? handler(state, action) : state
}
