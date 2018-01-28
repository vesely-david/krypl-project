import { authHeader, getTokenIfValid } from '../helpers'

export const authenticationService = {
  login,
  checkToken,
  logout
}

const addr = 'http://localhost:54849/api/client/'

function login (username, password) {
  const requestOptions = {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ username, password })
  }
  return fetch(addr + 'login', requestOptions)
    .then(response => {
      if (!response.ok) {
        return Promise.reject(response.statusText)
      }
      return response.json()
    })
    .then(tokenInfo => {
      // login successful if there's a jwt token in the response
      if (tokenInfo && tokenInfo.validUntil && tokenInfo.jwt) {
        // store jwt token in local storage to keep user logged in between page refreshes
        localStorage.setItem('tokenInfo', JSON.stringify(tokenInfo))
      }
      return tokenInfo
    })
}

function checkToken () {
  return getTokenIfValid()
}

function logout () {
  // remove user from local storage to log user out
  localStorage.removeItem('tokenInfo')
}
