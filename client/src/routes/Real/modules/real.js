import { strategyService } from '../../../services'

// ------------------------------------
// Constants
// ------------------------------------
export const REAL_OVERVIEW_REQUEST = 'REAL_OVERVIEW_REQUEST'
export const REAL_OVERVIEW_SUCCESS = 'REAL_OVERVIEW_SUCCESS'
export const REAL_OVERVIEW_ERROR = 'REAL_OVERVIEW_ERROR'
export const REAL_STRATEGIES_REQUEST = 'REAL_STRATEGIES_REQUEST'
export const REAL_STRATEGIES_SUCCESS = 'REAL_STRATEGIES_SUCCESS'
export const REAL_STRATEGIES_ERROR = 'REAL_STRATEGIES_ERROR'
export const REAL_FORGET_ALL_NEWS = 'REAL_FORGET_ALL_NEWS'
export const REAL_FORGET_NEWS_ERROR = 'REAL_FORGET_NEWS_ERROR'
export const REAL_REGISTER_STRATEGY_REQUEST = 'REAL_REGISTER_STRATEGY_REQUEST'
export const REAL_REGISTER_STRATEGY_SUCCESS = 'REAL_REGISTER_STRATEGY_SUCCESS'
export const REAL_REGISTER_STRATEGY_ERROR = 'REAL_REGISTER_STRATEGY_ERROR'

// ------------------------------------
// Actions
// ------------------------------------
export function fetchRealStrategies () {
  return async dispatch => {
    dispatch(realStrategiesRequest())
    function onSuccess (strategies) {
      dispatch(realStrategiesSuccess(strategies))
    }
    function onError (error) {
      dispatch(realStrategiesFailure(error))
    }

    try {
      const strategies = await strategyService.getRealStrategies()
      return onSuccess(strategies)
    } catch (error) {
      return onError(error)
    }
  }

  function realStrategiesRequest () { return { type: REAL_STRATEGIES_REQUEST } }
  function realStrategiesSuccess (strategies) { return { type: REAL_STRATEGIES_SUCCESS, payload: strategies } }
  function realStrategiesFailure () { return { type: REAL_STRATEGIES_ERROR } }
}

export function fetchRealOverview () {
  return async dispatch => {
    dispatch(realOverviewRequest())
    function onSuccess (overview) {
      dispatch(realOverviewSuccess(overview))
    }
    function onError (error) {
      dispatch(realOverviewFailure())
    }

    try {
      const overview = await strategyService.getRealOverview()
      return onSuccess(overview)
    } catch (error) {
      return onError(error)
    }
  }

  function realOverviewRequest () { return { type: REAL_OVERVIEW_REQUEST } }
  function realOverviewSuccess (overview) { return { type: REAL_OVERVIEW_SUCCESS, payload: overview } }
  function realOverviewFailure () { return { type: REAL_OVERVIEW_ERROR } }
}

export function forgetAllRealNews () {
  strategyService.forgetAllRealNews()
  return {
    type: REAL_FORGET_ALL_NEWS
  }
}

export function forgetRealNews (id) {
  strategyService.forgetRealNews(id)
  return {
    type    : REAL_FORGET_NEWS_ERROR,
    payload : id
  }
}

export function registerReal (name, selectedExchange, description, assets) {
  return async dispatch => {
    dispatch(registerRequest())
    function onSuccess (id) {
      dispatch(registerSuccess())
      dispatch(fetchRealStrategies())
      dispatch(fetchRealOverview())
      return id
    }
    function onError (error) {
      dispatch(registerError())
      return -1
    }
    try {
      const id = await strategyService.registerReal(name, selectedExchange, description, assets)
      return onSuccess(id)
    } catch (error) {
      return onError(error)
    }
  }
  function registerRequest () { return { type: REAL_REGISTER_STRATEGY_REQUEST } }
  function registerSuccess () { return { type: REAL_REGISTER_STRATEGY_SUCCESS } }
  function registerError (error) { return { type: REAL_REGISTER_STRATEGY_ERROR, payload: error } }
}

export const realActions = {
  fetchRealStrategies,
  fetchRealOverview,
  forgetAllRealNews,
  forgetRealNews,
  registerReal,
}

// ------------------------------------
// Action Handlers
// ------------------------------------
const ACTION_HANDLERS = {
  [REAL_OVERVIEW_REQUEST]  : (state, action) => ({ ...state, overviewFetching: true }),
  [REAL_OVERVIEW_SUCCESS]  : (state, action) => (
    { ...state, overviewFetching: false, overview: action.payload }),
  [REAL_OVERVIEW_ERROR]    : (state, action) => ({ ...state, overviewFetching: false }),

  [REAL_STRATEGIES_REQUEST]  : (state, action) => ({ ...state, strategiesFetching: true }),
  [REAL_STRATEGIES_SUCCESS]  : (state, action) => (
    { ...state, strategiesFetching: false, strategyList: action.payload }),
  [REAL_STRATEGIES_ERROR]    : (state, action) => ({ ...state, strategiesFetching: false }),

  [REAL_REGISTER_STRATEGY_REQUEST]  : (state, action) => ({ ...state, registrationPending: true }),
  [REAL_REGISTER_STRATEGY_SUCCESS]  : (state, action) => ({
    ...state,
    registrationPending: false,
    // strategyList: [action.payload, ...state.strategyList] // Add new strategy to the list
  }),
  [REAL_REGISTER_STRATEGY_ERROR]    : (state, action) => ({ ...state, registrationPending: false }),

  [REAL_FORGET_ALL_NEWS]   : (state, action) => ({
    ...state,
    strategyList: state.strategyList.map(o => ({ ...o, newTrades: 0 }))
  }),
  [REAL_FORGET_NEWS_ERROR] : (state, action) => ({
    ...state,
    strategyList: state.strategyList.map(o => {
      if (o.id === action.payload) return { ...o, newTrades: 0 }
      else return o
    })
  }),
}

// ------------------------------------
// Reducer
// ------------------------------------
const initialState = {
  overviewFetching: false,
  strategiesFetching: false,
  registrationPending: false,
  strategyList: [],
  overview: {
    overview: {},
    assets: [],
  }
}
export default function counterReducer (state = initialState, action) {
  const handler = ACTION_HANDLERS[action.type]
  return handler ? handler(state, action) : state
}
