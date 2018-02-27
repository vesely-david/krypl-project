import { injectReducer } from '../../store/reducers'

export default (store) => ({
  path : 'strategy/:strategyId',
  /*  Async getComponent is only invoked when route matches   */
  getComponent (nextState, cb) {
    /*  Webpack - use 'require.ensure' to create a split point
        and embed an async module loader (jsonp) when bundling   */
    require.ensure([], (require) => {
      /*  Webpack - use require callback to define
          dependencies for bundling   */
      const Strategy = require('./containers/StrategyContainer').default
      //const reducer = require('./modules/backtest').default

      /*  Add the reducer to the store on key 'backtest'  */
      //injectReducer(store, { key: 'backtest', reducer })

      /*  Return getComponent   */
      cb(null, Strategy)

    /* Webpack named bundle   */
    }, 'strategy')
  }
})
