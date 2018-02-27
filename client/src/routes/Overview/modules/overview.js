import { strategyService } from '../../../services'
import {
  REAL_OVERVIEW_REQUEST,
  REAL_OVERVIEW_SUCCESS,
  REAL_OVERVIEW_ERROR
} from '../../Real/modules/real'

// ------------------------------------
// Constants
// ------------------------------------
export const MAIN_OVERVIEW_REQUEST = 'MAIN_OVERVIEW_REQUEST'
export const MAIN_OVERVIEW_SUCCESS = 'MAIN_OVERVIEW_SUCCESS'
export const MAIN_OVERVIEW_ERROR = 'MAIN_OVERVIEW_ERROR'

export const ASSET_OPTIONS_REQUEST = 'ASSET_OPTIONS_REQUEST'
export const ASSET_OPTIONS_SUCCESS = 'ASSET_OPTIONS_SUCCESS'
export const ASSET_OPTIONS_ERROR = 'ASSET_OPTIONS_ERROR'

// ------------------------------------
// Actions
// ------------------------------------

export function fetchMainOverview () {
  return async dispatch => {
    dispatch(mainOverviewRequest())
    function onSuccess (mainOverview) {
      dispatch(mainOverviewSuccess(mainOverview))
    }
    function onError (error) {
      dispatch(mainOverviewFailure(error))
    }

    try {
      const mainOverview = await strategyService.getMainOverview()
      return onSuccess(mainOverview)
    } catch (error) {
      return onError(error)
    }
  }

  function mainOverviewRequest () { return { type: MAIN_OVERVIEW_REQUEST } }
  function mainOverviewSuccess (mainOverview) { return { type: MAIN_OVERVIEW_SUCCESS, payload: mainOverview } }
  function mainOverviewFailure () { return { type: MAIN_OVERVIEW_ERROR } }
}

export function fetchAssetOptions () {
  return async dispatch => {
    dispatch(assetOptionsRequest())
    function onSuccess (assetOptions) {
      dispatch(assetOptionsSuccess(assetOptions))
    }
    function onError (error) {
      dispatch(assetOptionsFailure(error))
    }

    try {
      const assetOptions = await strategyService.getMainOverview()
      return onSuccess(assetOptions)
    } catch (error) {
      return onError(error)
    }
  }

  function assetOptionsRequest () { return { type: MAIN_OVERVIEW_REQUEST } }
  function assetOptionsSuccess (assetOptions) { return { type: MAIN_OVERVIEW_SUCCESS, payload: assetOptions } }
  function assetOptionsFailure () { return { type: MAIN_OVERVIEW_ERROR } }
}

export const actions = {
  fetchMainOverview,
  fetchAssetOptions,
}

// ------------------------------------
// Action Handlers
// ------------------------------------
const ACTION_HANDLERS = {
  [MAIN_OVERVIEW_REQUEST]    : (state, action) => ({ ...state, isFetching: true }),
  [MAIN_OVERVIEW_SUCCESS]    : (state, action) => ({ ...state, isFetching: false }),
  [MAIN_OVERVIEW_ERROR]      : (state, action) => ({ ...state, isFetching: false }),

  [REAL_OVERVIEW_REQUEST]    : (state, action) => ({ ...state, isFetching: true }),
  [REAL_OVERVIEW_SUCCESS]    : (state, action) => ({ ...state, isFetching: false }),
  [REAL_OVERVIEW_ERROR]      : (state, action) => ({ ...state, isFetching: false }),

  [ASSET_OPTIONS_REQUEST]    : (state, action) => ({ ...state, assetOptionsFetching: true }),
  [ASSET_OPTIONS_SUCCESS]    : (state, action) =>
    ({ ...state, assetOptions: action.payload, assetOptionsFetching: false }),
  [ASSET_OPTIONS_ERROR]      : (state, action) => ({ ...state, assetOptionsFetching: false })
}

// ------------------------------------
// Reducer
// ------------------------------------
const initialState = {
  isFetching: false,
  assetOptions: null,
  assetOptionsFetching: false,
  assetUpdateFetching: false,
}
export default function overviewReducer (state = initialState, action) {
  const handler = ACTION_HANDLERS[action.type]
  return handler ? handler(state, action) : state
}
