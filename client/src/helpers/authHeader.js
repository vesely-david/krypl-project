export function authHeader () {
  // return authorization header with jwt token
  const jwt = getTokenIfValid()
  if (jwt) {
    return { 'Authorization': 'Bearer ' + jwt }
  } else {
    return {}
  }
}

export function getTokenIfValid () {
  let tokenInfo = JSON.parse(localStorage.getItem('tokenInfo'))

  if (tokenInfo && tokenInfo.validUntil && tokenInfo.jwt) {
    return parseInt(tokenInfo.validUntil) > (Date.now() / 1000) ? tokenInfo.jwt : null
  }
  return null
}
