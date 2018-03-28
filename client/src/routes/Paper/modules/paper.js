import { strategyService } from '../../../services'
// ------------------------------------
// Constants
// ------------------------------------
export const PAPER_OVERVIEW_REQUEST = 'PAPER_OVERVIEW_REQUEST'
export const PAPER_OVERVIEW_SUCCESS = 'PAPER_OVERVIEW_SUCCESS'
export const PAPER_OVERVIEW_ERROR = 'PAPER_OVERVIEW_ERROR'
export const PAPER_STRATEGIES_REQUEST = 'PAPER_STRATEGIES_REQUEST'
export const PAPER_STRATEGIES_SUCCESS = 'PAPER_STRATEGIES_SUCCESS'
export const PAPER_STRATEGIES_ERROR = 'PAPER_STRATEGIES_ERROR'
export const PAPER_FORGET_ALL_NEWS = 'PAPER_FORGET_ALL_NEWS'
export const PAPER_FORGET_NEWS_ERROR = 'PAPER_FORGET_NEWS_ERROR'
export const PAPER_REGISTER_STRATEGY_REQUEST = 'PAPER_REGISTER_STRATEGY_REQUEST'
export const PAPER_REGISTER_STRATEGY_SUCCESS = 'PAPER_REGISTER_STRATEGY_SUCCESS'
export const PAPER_REGISTER_STRATEGY_ERROR = 'PAPER_REGISTER_STRATEGY_ERROR'

// ------------------------------------
// Actions
// ------------------------------------
export function fetchPaperStrategies () {
  return async dispatch => {
    dispatch(paperStrategiesRequest())
    function onSuccess (strategies) {
      dispatch(paperStrategiesSuccess(strategies))
    }
    function onError (error) {
      dispatch(paperStrategiesFailure(error))
    }

    try {
      const strategies = await strategyService.getPealStrategies()
      return onSuccess(strategies)
    } catch (error) {
      return onError(error)
    }
  }

  function paperStrategiesRequest () { return { type: PAPER_STRATEGIES_REQUEST } }
  function paperStrategiesSuccess (strategies) { return { type: PAPER_STRATEGIES_SUCCESS, payload: strategies } }
  function paperStrategiesFailure () { return { type: PAPER_STRATEGIES_ERROR } }
}

export function fetchPaperOverview () {
  return async dispatch => {
    dispatch(paperOverviewRequest())
    function onSuccess (overview) {
      dispatch(paperOverviewSuccess(overview))
    }
    function onError (error) {
      dispatch(paperOverviewFailure())
    }

    try {
      const overview = await strategyService.getPaperOverview()
      return onSuccess(overview)
    } catch (error) {
      return onError(error)
    }
  }

  function paperOverviewRequest () { return { type: PAPER_OVERVIEW_REQUEST } }
  function paperOverviewSuccess (overview) { return { type: PAPER_OVERVIEW_SUCCESS, payload: overview } }
  function paperOverviewFailure () { return { type: PAPER_OVERVIEW_ERROR } }
}

export function forgetAllPaperNews () {
  strategyService.forgetAllPaperNews()
  return {
    type: PAPER_FORGET_ALL_NEWS
  }
}

export function forgetPaperNews (id) {
  strategyService.forgetPaperNews(id)
  return {
    type    : PAPER_FORGET_NEWS_ERROR,
    payload : id
  }
}

export function registerPaper (name, selectedExchange, description, assets) {
  return async dispatch => {
    dispatch(registerRequest())
    function onSuccess (id) {
      dispatch(registerSuccess())
      dispatch(fetchPaperStrategies())
      dispatch(fetchPaperOverview())
      return id
    }
    function onError (error) {
      dispatch(registerError())
      return -1
    }
    try {
      const id = await strategyService.registerPaper(name, selectedExchange, description, assets)
      return onSuccess(id)
    } catch (error) {
      return onError(error)
    }
  }
  function registerRequest () { return { type: PAPER_REGISTER_STRATEGY_REQUEST } }
  function registerSuccess () { return { type: PAPER_REGISTER_STRATEGY_SUCCESS } }
  function registerError (error) { return { type: PAPER_REGISTER_STRATEGY_ERROR, payload: error } }
}

export const paperActions = {
  fetchPaperStrategies,
  fetchPaperOverview,
  forgetAllPaperNews,
  forgetPaperNews,
  registerPaper,
}

// ------------------------------------
// Action Handlers
// ------------------------------------
const ACTION_HANDLERS = {
  [PAPER_OVERVIEW_REQUEST]  : (state, action) => ({ ...state, overviewFetching: true }),
  [PAPER_OVERVIEW_SUCCESS]  : (state, action) => (
    { ...state, overviewFetching: false, overview: action.payload }),
  [PAPER_OVERVIEW_ERROR]    : (state, action) => ({ ...state, overviewFetching: false }),

  [PAPER_STRATEGIES_REQUEST]  : (state, action) => ({ ...state, strategiesFetching: true }),
  [PAPER_STRATEGIES_SUCCESS]  : (state, action) => (
    { ...state, strategiesFetching: false, strategyList: action.payload }),
  [PAPER_STRATEGIES_ERROR]    : (state, action) => ({ ...state, strategiesFetching: false }),

  [PAPER_REGISTER_STRATEGY_REQUEST]  : (state, action) => ({ ...state, registrationPending: true }),
  [PAPER_REGISTER_STRATEGY_SUCCESS]  : (state, action) => ({
    ...state,
    registrationPending: false,
    // strategyList: [action.payload, ...state.strategyList] // Add new strategy to the list
  }),
  [PAPER_REGISTER_STRATEGY_ERROR]    : (state, action) => ({ ...state, registrationPending: false }),

  [PAPER_FORGET_ALL_NEWS]   : (state, action) => ({
    ...state,
    strategyList: state.strategyList.map(o => ({ ...o, newTrades: 0 }))
  }),
  [PAPER_FORGET_NEWS_ERROR] : (state, action) => ({
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
export default function paperReducer (state = initialState, action) {
  const handler = ACTION_HANDLERS[action.type]
  return handler ? handler(state, action) : state
}
