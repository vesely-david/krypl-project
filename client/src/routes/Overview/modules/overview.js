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

// ------------------------------------
// Actions
// ------------------------------------

export function fetchMainOverview () {
  return async dispatch => {
    dispatch(mainOverviewRequest())
    function onSuccess (strategies) {
      dispatch(mainOverviewSuccess(strategies))
    }
    function onError (error) {
      dispatch(mainOverviewFailure(error))
    }

    try {
      const strategies = await strategyService.getMainOverview()
      return onSuccess(strategies)
    } catch (error) {
      return onError(error)
    }
  }

  function mainOverviewRequest () { return { type: MAIN_OVERVIEW_REQUEST } }
  function mainOverviewSuccess (mainOverview) { return { type: MAIN_OVERVIEW_SUCCESS, payload: mainOverview } }
  function mainOverviewFailure () { return { type: MAIN_OVERVIEW_ERROR } }
}

export const actions = {
  fetchMainOverview
}

// ------------------------------------
// Action Handlers
// ------------------------------------
const ACTION_HANDLERS = {
  [MAIN_OVERVIEW_REQUEST]    : (state, action) => ({ isFetching: true }),
  [MAIN_OVERVIEW_SUCCESS]    : (state, action) => ({ isFetching: false }),
  [MAIN_OVERVIEW_ERROR]      : (state, action) => ({ isFetching: false }),

  [REAL_OVERVIEW_REQUEST]    : (state, action) => ({ isFetching: true }),
  [REAL_OVERVIEW_SUCCESS]    : (state, action) => ({ isFetching: false }),
  [REAL_OVERVIEW_ERROR]      : (state, action) => ({ isFetching: false })
}

// ------------------------------------
// Reducer
// ------------------------------------
const initialState = {
  isFetching: false,
}
export default function counterReducer (state = initialState, action) {
  const handler = ACTION_HANDLERS[action.type]

  return handler ? handler(state, action) : state
}
