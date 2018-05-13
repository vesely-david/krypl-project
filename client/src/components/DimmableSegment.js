import React from 'react'
import PropTypes from 'prop-types'
import { Segment, Dimmer, Loader } from 'semantic-ui-react'

const DimmableSegment = ({ active, children }) => (
  <Segment>
    <Dimmer inverted active={active}>
      <Loader />
    </Dimmer>
    {children}
  </Segment>
)

DimmableSegment.propTypes = {
  active: PropTypes.bool,
  children: PropTypes.node,
}

export default DimmableSegment
