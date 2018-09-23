import { visualizationService } from '../../../services'

// ------------------------------------
// Constants
// ------------------------------------
export const FETCH_TRADES_REQUEST = 'FETCH_TRADES_REQUEST'
export const FETCH_TRADES_SUCCESS = 'FETCH_TRADES_SUCCESS'
export const FETCH_TRADES_ERROR = 'FETCH_TRADES_ERROR'

export const FETCH_STRATEGY_OVERVIEW_REQUEST = 'FETCH_STRATEGY_OVERVIEW_REQUEST'
export const FETCH_STRATEGY_OVERVIEW_SUCCESS = 'FETCH_STRATEGY_OVERVIEW_SUCCESS'
export const FETCH_STRATEGY_OVERVIEW_ERROR = 'FETCH_STRATEGY_OVERVIEW_ERROR'

export const COMMAND_REQUEST = 'COMMAND_REQUEST'
export const COMMAND_SUCCESS = 'COMMAND_SUCCESS'
export const COMMAND_ERROR = 'COMMAND_ERROR'

// ------------------------------------
// Actions
// ------------------------------------

export function fetchStrategyOverview (id) {
  return async dispatch => {
    dispatch(fetchStrategyOverviewRequest())
    function onSuccess (overview) {
      dispatch(fetchStrategyOverviewSuccess(overview))
    }
    function onError () {
      dispatch(fetchStrategyOverviewError())
    }
    try {
      const overview = await visualizationService.fetchStrategyOverview(id)
      return onSuccess(overview)
    } catch (error) {
      return onError()
    }
  }
  function fetchStrategyOverviewRequest () { return { type: FETCH_STRATEGY_OVERVIEW_REQUEST } }
  function fetchStrategyOverviewSuccess (overview) { return { type: FETCH_STRATEGY_OVERVIEW_SUCCESS, payload: overview } }
  function fetchStrategyOverviewError () { return { type: FETCH_STRATEGY_OVERVIEW_ERROR } }
}

export function fetchStrategyTrades (id) {
  return async dispatch => {
    dispatch(fetchTradesRequest())
    function onSuccess (trades) {
      dispatch(fetchTradesSuccess(trades))
    }
    function onError () {
      dispatch(fetchTradesError())
    }
    try {
      const trades = await visualizationService.fetchStrategyTrades(id)
      return onSuccess(trades)
    } catch (error) {
      return onError()
    }
  }
  function fetchTradesRequest () { return { type: FETCH_TRADES_REQUEST } }
  function fetchTradesSuccess (trades) { return { type: FETCH_TRADES_SUCCESS, payload: trades } }
  function fetchTradesError () { return { type: FETCH_TRADES_ERROR } }
}

export const actions = {
  fetchStrategyOverview,
  fetchStrategyTrades,
  //stopStrategy
}

// ------------------------------------
// Action Handlers
// ------------------------------------
const ACTION_HANDLERS = {
  [FETCH_STRATEGY_OVERVIEW_REQUEST] : (state, action) => ({ ...state, overviewFetching: true }),
  [FETCH_STRATEGY_OVERVIEW_SUCCESS] : (state, action) =>
    ({ ...state, overviewFetching: false, overview: action.payload }),
  [FETCH_STRATEGY_OVERVIEW_ERROR] : (state, action) => ({ ...state, overviewFetching: false }),

  [FETCH_TRADES_REQUEST] : (state, action) => ({ ...state, tradesFetching: true }),
  [FETCH_TRADES_SUCCESS] : (state, action) =>
    ({ ...state, tradesFetching: false, trades: action.payload }),
  [FETCH_TRADES_ERROR] : (state, action) => ({ ...state, tradesFetching: false }),

  // [EXCHANGES_OVERVIEW_REQUEST]  : (state, action) => ({ ...state, exchangesOverviewFetching: true }),
  // [EXCHANGES_OVERVIEW_SUCCESS]  : (state, action) => (
  //   { ...state, exchangesOverviewFetching: false, exchangesOverview: action.payload }
  // ),
  // [EXCHANGES_OVERVIEW_ERROR]    : (state, action) => ({ ...state, exchangesOverviewFetching: false }),
}

// ------------------------------------
// Reducer
// ------------------------------------
const initialState = {
  overview: {},
  trades: [],
  overviewFetching: false,
  tradesFetching: false,
  commandPending: false,
}
export default function strategyReducer (state = initialState, action) {
  const handler = ACTION_HANDLERS[action.type]
  return handler ? handler(state, action) : state
}
