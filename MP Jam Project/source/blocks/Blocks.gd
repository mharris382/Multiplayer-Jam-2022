extends Node2D

#Global Singleton providing access to the most important parts of the Blocks Module

#Character Id: 1=Transporter, 2=Builder
signal character_collected_block(character_id, block_id, dynamic_block)

signal builder_built_block(builder, block_id, grid_position)
signal transporter_disconnected_block(transporter, block_id, grid_position)
signal transporter_threw_block(transporter, dynamic_block, throw_force)
