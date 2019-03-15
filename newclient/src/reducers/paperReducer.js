import {
  GET_PAPER_OVERVIEW,
  GET_PAPER_STRATEGIES
} from '../actions/types';


const ACTION_HANDLERS = {
  [`${GET_PAPER_OVERVIEW}_PENDING`] : (state, action) => {
    return {...state, overviewFetching: true}
  },
  [`${GET_PAPER_OVERVIEW}_FULFILLED`] : (state, action) => {
    return {
      ...state,
      overview: action.payload,
      overviewFetching: false
    }
  },
  [`${GET_PAPER_OVERVIEW}_REJECTED`] : (state, action) => ({
    ...state, 
    overviewFetching: false
  }),
//==========================================================
  [`${GET_PAPER_STRATEGIES}_PENDING`] : (state, action) => {
    return {...state, strategiesFetching: true}
  },
  [`${GET_PAPER_STRATEGIES}_FULFILLED`] : (state, action) => {
    return {
      ...state,
      strategyList: action.payload,
      strategiesFetching: false
    }
  },
  [`${GET_PAPER_STRATEGIES}_REJECTED`] : (state, action) => ({
    ...state, 
    strategiesFetching: false
  }),
}

const initialState = {
  strategyList: {
    strategies: [],
    page: 0,
  },
  overview: {},
  registrationPending: false,
  strategiesFetching: false,
  overviewFetching: false,
}

export default function paperReducer (state = initialState, action) {
  const handler = ACTION_HANDLERS[action.type]
  return handler ? handler(state, action) : state
}
