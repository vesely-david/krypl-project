import { injectReducer } from '../../store/reducers'

export default (store) => ({
  getComponent (nextState, cb) {
    /*  Webpack - use 'require.ensure' to create a split point
        and embed an async module loader (jsonp) when bundling   */
    require.ensure([], (require) => {
      /*  Webpack - use require callback to define
          dependencies for bundling   */
      const Overview = require('./containers/OverviewContainer').default
      const reducer = require('./modules/overview').default

      /*  Add the reducer to the store on key 'overview'  */
      injectReducer(store, { key: 'overview', reducer })

      /*  Return getComponent   */
      cb(null, Overview)

    /* Webpack named bundle   */
    }, 'overview')
  }
})
