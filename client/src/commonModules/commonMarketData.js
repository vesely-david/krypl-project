import { marketDataService } from '../services'

// ------------------------------------
// Constants
// ------------------------------------
export const FETCH_MARKET_DATA_REQUEST = 'FETCH_MARKET_DATA_REQUEST'
export const FETCH_MARKET_DATA_SUCCESS = 'FETCH_MARKET_DATA_SUCCESS'
export const FETCH_MARKET_DATA_ERROR = 'FETCH_MARKET_DATA_ERROR'

// ------------------------------------
// Actions
// ------------------------------------
export function fetchCommonMarketData () {
  return async (dispatch, getState) => {
    function onSuccess (marketData) {
      dispatch(marketDataSuccess(marketData))
    }
    function onError (error) {
      dispatch(marketDataFailure())
    }
    var state = getState()

    if (!state.marketData.marketData && !state.marketData.marketDataFetching) {
      dispatch(marketDataRequest())
      try {
        const marketData = await marketDataService.fetchMarketData()
        return onSuccess(marketData)
      } catch (error) {
        return onError(error)
      }
    }
  }

  function marketDataRequest () { return { type: FETCH_MARKET_DATA_REQUEST } }
  function marketDataSuccess (marketData) { return { type: FETCH_MARKET_DATA_SUCCESS, payload: marketData } }
  function marketDataFailure () { return { type: FETCH_MARKET_DATA_ERROR } }
}

export const commonMarketDataActions = {
  fetchCommonMarketData,
}

// ------------------------------------
// Action Handlers
// ------------------------------------
const ACTION_HANDLERS = {
  [FETCH_MARKET_DATA_REQUEST] : (state, action) => ({ ...state, marketDataFetching: true }),
  [FETCH_MARKET_DATA_SUCCESS] : (state, action) =>
    ({ ...state, marketDataFetching: false, marketData: action.payload }),
  [FETCH_MARKET_DATA_ERROR] : (state, action) => ({ ...state, marketDataFetching: false }),
}

// ------------------------------------
// Reducer
// ------------------------------------
const initialState = {
  marketDataFetching: false,
  marketData: null
}

export default function authenticationReducer (state = initialState, action) {
  const handler = ACTION_HANDLERS[action.type]
  return handler ? handler(state, action) : state
}
