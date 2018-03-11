import { balanceService, strategyService } from '../../../services'

// ------------------------------------
// Constants
// ------------------------------------
export const EXCHANGES_OVERVIEW_REQUEST = 'EXCHANGES_OVERVIEW_REQUEST'
export const EXCHANGES_OVERVIEW_SUCCESS = 'EXCHANGES_OVERVIEW_SUCCESS'
export const EXCHANGES_OVERVIEW_ERROR = 'EXCHANGES_OVERVIEW_ERROR'

export const MIRROR_ASSETS_REQUEST = 'MIRROR_ASSETS_REQUEST'
export const MIRROR_ASSETS_SUCCESS = 'MIRROR_ASSETS_SUCCESS'
export const MIRROR_ASSETS_ERROR = 'MIRROR_ASSETS_ERROR'

// ------------------------------------
// Actions
// ------------------------------------

export function mirrorExchangeAssets (exchange) {
  return async dispatch => {
    dispatch(mirrorAssetsRequest())
    function onSuccess () {
      dispatch(mirrorAssetsSuccess())
      return null
    }
    function onError (errore) {
      dispatch(mirrorAssetsFailure())
      return errore
    }

    try {
      await balanceService.mirrorRealAssets(exchange)
      return onSuccess()
    } catch (error) {
      return onError(error.message)
    }
  }
  function mirrorAssetsRequest () { return { type: MIRROR_ASSETS_REQUEST } }
  function mirrorAssetsSuccess () { return { type: MIRROR_ASSETS_SUCCESS } }
  function mirrorAssetsFailure () { return { type: MIRROR_ASSETS_ERROR } }
}

export function fetchExchangesOverview () {
  return async dispatch => {
    dispatch(exchangeOverviewRequest())
    function onSuccess (exchangesOverview) {
      dispatch(exchangeOverviewSuccess(exchangesOverview))
    }
    function onError (error) {
      dispatch(exchangeOverviewFailure(error))
    }

    try {
      const exchangesOverview = await balanceService.getExchangesOverview()
      return onSuccess(exchangesOverview)
    } catch (error) {
      return onError(error)
    }
  }

  function exchangeOverviewRequest () { return { type: EXCHANGES_OVERVIEW_REQUEST } }
  function exchangeOverviewSuccess (exchangesOverview) { return { type: EXCHANGES_OVERVIEW_SUCCESS, payload: exchangesOverview } }
  function exchangeOverviewFailure () { return { type: EXCHANGES_OVERVIEW_ERROR } }
}

export const actions = {
  fetchExchangesOverview,
  mirrorExchangeAssets,
}

// ------------------------------------
// Action Handlers
// ------------------------------------
const ACTION_HANDLERS = {
  [EXCHANGES_OVERVIEW_REQUEST]  : (state, action) => ({ ...state, exchangesOverviewFetching: true }),
  [EXCHANGES_OVERVIEW_SUCCESS]  : (state, action) => (
    { ...state, exchangesOverviewFetching: false, exchangesOverview: action.payload }
  ),
  [EXCHANGES_OVERVIEW_ERROR]    : (state, action) => ({ ...state, exchangesOverviewFetching: false }),

  [MIRROR_ASSETS_REQUEST]       : (state, action) => ({ ...state, mirrorExchangeAssetsFetching: true }),
  [MIRROR_ASSETS_SUCCESS]       : (state, action) => ({ ...state, mirrorExchangeAssetsFetching: false }),
  [MIRROR_ASSETS_ERROR]         : (state, action) => ({ ...state, mirrorExchangeAssetsFetching: false }),
}

// ------------------------------------
// Reducer
// ------------------------------------
const initialState = {
  exchangesOverview: {
    real: null,
    paper: null,
    backtest: null
  },
  exchangesOverviewFetching: false,
  mirrorExchangeAssetsFetching: false
}
export default function overviewReducer (state = initialState, action) {
  const handler = ACTION_HANDLERS[action.type]
  return handler ? handler(state, action) : state
}
