import {
  GET_REAL_OVERVIEW,
  GET_REAL_STRATEGIES,
  GET_REAL_STRATEGY,
} from '../actions/types';


const ACTION_HANDLERS = {
  [`${GET_REAL_STRATEGIES}_PENDING`] : (state, action) => {
    return {
      ...state, 
      strategiesFetching: true
    }
  },
  [`${GET_REAL_STRATEGIES}_FULFILLED`] : (state, action) => {
    return {
      ...state,
      strategiesFetching: false,
      strategies: action.payload.strategies,
      pagination: action.payload.pagination,
    }
  },
  [`${GET_REAL_STRATEGIES}_REJECTED`] : (state, action) => {
    return {
      ...state, 
      strategiesFetching: false
    }
  },
  //====================================================     
  [`${GET_REAL_STRATEGY}_PENDING`] : (state, action) => {
    return {
      ...state, 
      strategiesFetching: true
    }
  },
  [`${GET_REAL_STRATEGY}_FULFILLED`] : (state, action) => {
    const original = state.strategies.find(o => o.id === action.payload.id);
    return {
      ...state,
      strategiesFetching: false,
      strategies: original ? 
      state.strategies.map(o => o.id === action.payload.id ? action.payload : o) : [action.payload],
      pagination: original ? state.pagination : {page: 0, perPage: 30, total: 0},
    }
  },
  [`${GET_REAL_STRATEGY}_REJECTED`] : (state, action) => {
    return {
      ...state, 
      strategiesFetching: false
    }
  },
  //====================================================       
  [`${GET_REAL_OVERVIEW}_PENDING`] : (state, action) => {
    return {...state, overviewFetching: true}
  },
  [`${GET_REAL_OVERVIEW}_FULFILLED`] : (state, action) => {
    return {
      ...state,
      overview: action.payload,
      overviewFetching: false
    }
  },
  [`${GET_REAL_OVERVIEW}_REJECTED`] : (state, action) => ({
    ...state, 
    overviewFetching: false
  }),
}

const initialState = {
  strategies: [],
  pagination: {page: 0, perPage: 30, total: 0},
  overview: {},
  registrationPending: false,
  strategiesFetching: false,
  overviewFetching: false,
}

export default function paperReducer (state = initialState, action) {
  const handler = ACTION_HANDLERS[action.type]
  return handler ? handler(state, action) : state
}
