import { authenticationService } from '../services'
import { history } from '../helpers'

// ------------------------------------
// Constants
// ------------------------------------
export const AUTHENTICATION_REQUEST = 'AUTHENTICATION_REQUEST'
export const AUTHENTICATED_TRUE = 'AUTHENTICATED_TRUE'
export const AUTHENTICATED_FALSE = 'AUTHENTICATED_FALSE'

// ------------------------------------
// Actions
// ------------------------------------
export function checkTokenValidity (token) {
  return authenticationService.checkToken() ? {
    type: AUTHENTICATED_TRUE
  } : {
    type: AUTHENTICATED_FALSE
  }
}

export function login (username, password) {
  return dispatch => {
    dispatch(loginRequest())
    authenticationService.login(username, password)
      .then(
          success => {
            dispatch(loginSuccess())
            history.push('/')
          },
          error => {
            dispatch(loginFailure(error))
          }
      )
  }

  function loginRequest () { return { type: AUTHENTICATION_REQUEST } }
  function loginSuccess () { return { type: AUTHENTICATED_TRUE } }
  function loginFailure (error) { return { type: AUTHENTICATED_FALSE, payload: error } }
}

export function logout () {
  authenticationService.logout()
  history.push('/login')
  return { type: AUTHENTICATED_FALSE }
}

export const authenticationActions = {
  checkTokenValidity,
  login,
  logout,
}

// ------------------------------------
// Action Handlers
// ------------------------------------
const ACTION_HANDLERS = {
  [AUTHENTICATION_REQUEST] : (state, action) => null,
  [AUTHENTICATED_TRUE] : (state, action) => true,
  [AUTHENTICATED_FALSE] : (state, action) => false,
}

// ------------------------------------
// Reducer
// ------------------------------------
const initialState = false

export default function authenticationReducer (state = initialState, action) {
  const handler = ACTION_HANDLERS[action.type]
  return handler ? handler(state, action) : state
}
